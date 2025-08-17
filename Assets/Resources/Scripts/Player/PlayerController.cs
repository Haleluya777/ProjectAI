using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlayerController : MonoBehaviour, IDamageable, ISkillCaster
{
    private enum State { Idle, Moving, Dash, Attacking, Jumping, Climbing }
    public Vector3 respawn;
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    private enum AttackType { Physical, Magical }
    [SerializeField] private Transform center;

    //레이캐스트 설정----
    private RaycastHit2D checkingWall;
    private RaycastHit2D checkingGround;
    private List<RaycastHit2D> allRayCastHits = new List<RaycastHit2D>();
    private ContactFilter2D contactFilter; //레이어, isTrigger필터

    //레이어 마스크에 쓸 레이어들.
    private const int PLATFORM_LAYER = 6;
    private const int INTERACTIVE_OBJECT_LAYER = 3;
    private const int CAN_CLIMB_WALL = 8;
    //----------

    private IInteractable interactable; //상호작용 가능한 오브젝트.
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private State currentState;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private GameObject particle;

    //플레이어 기본 스탯 및 상태들.
    private int level;
    private int maxHp, curHp;
    private float maxStm, curStm;
    [SerializeField] private int curMoveSpeed;
    [SerializeField] private int jumpPower;
    private int holdJumpPower;
    private float scaleX = 1;
    private float scaleY = 1;
    private int combo;
    private bool isdead;
    private bool canJump;
    private bool canDamaged;
    private float maxjumpHoldTime;
    [SerializeField] private int att, defense, magicalDefense;
    [SerializeField] private bool attacking;
    [SerializeField] private bool castingSkill;
    //

    public int CurrentHp => curHp;
    public int Att => att;
    public int TotalDmg { get; set; }
    public bool IsDead => isdead;
    public float Scale => scaleX;
    public bool CanAction { get; set; }
    private Vector3 dir;
    private float moveX;
    private float moveY;
    private float regenSpeed;
    private bool canRegen;
    [SerializeField] private bool delayed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float curcoyoteTime;
    [SerializeField] private Vector2 momentum;
    private float momentumX, momentumY;
    private float attCool, attTime;
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Coroutine newCorutine;
    private Coroutine comboCoroutine;
    private const int WALK_SPEED = 15;
    private const int GRAVITY_SCALE = 12;
    public Vector3 Dir => dir;
    public bool overground;
    [SerializeField] public float gracePeriod;
    private Coroutine GraceTimeCoroutine;
    private WaitForSeconds gp;
    [SerializeField] private bool damagabool;
    [SerializeField] private Skill_Module currentSkill;


    //콜라이더 크기 조정
    private Vector2 climbingColSize;
    private Vector2 defaultColSize;
    private Vector2 checkingGroundCastSize;

    void Start()
    {
        StatusInit();

        climbingColSize = new Vector2(4, 2);
        defaultColSize = new Vector2(2, 4);
        checkingGroundCastSize = new Vector2(1.5f, .1f);

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(1 << INTERACTIVE_OBJECT_LAYER);
        contactFilter.useTriggers = true;

        gp = new WaitForSeconds(gracePeriod);
    }

    void Update()
    {
        if (CanAction)
        {
            currentState = StateUpdate();
            StateAction(currentState);
        }

        moveX = Input.GetAxisRaw("Horizontal");
        if (currentState == State.Idle) { canRegen = true; } else { canRegen = false; }

        ProccessCoyoteTime();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        col.size = currentState != State.Climbing ? defaultColSize : climbingColSize;

        InputAttack();
        UseSkill();
        ProccessingPassive();
        StmRegen();
        PlayerUIUpdate();
        InteractiveObject();

        // 스킬 모듈의 쿨다운을 매 프레임 업데이트
        if (currentSkill != null)
        {
            currentSkill.UpdateCoolDown(Time.deltaTime);
        }

        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curStm = Mathf.Clamp(curStm, 0, maxStm);

        //플레이어 상태이상 확인용
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StatusEffectProcess(3f, "Stun");
        }
    }

    private void FixedUpdate()
    {
        //붙을 수 있는 벽을 탐지하는 레이캐스트
        checkingWall = Physics2D.BoxCast(col.bounds.center, new Vector2(.5f, col.bounds.size.y - .1f), 0, transform.right * dir.x, 2f, 1 << CAN_CLIMB_WALL);
        //주변 상호작용 가능한 오브젝트들을 모드 탐지하는 박스 캐스트.
        Physics2D.BoxCast(col.bounds.center, new Vector2(col.bounds.size.x + .1f, col.bounds.size.y + .1f), 0, Vector2.zero, contactFilter, allRayCastHits, 0f);
        //플랫폼에 발을 딛고 있는지 체크하는 박스 캐스트.
        checkingGround = Physics2D.BoxCast(transform.position, checkingGroundCastSize, 0, Vector2.zero, 0.1f, 1 << PLATFORM_LAYER);
        Debug.DrawRay(col.bounds.center, transform.right * dir.x * 2f, Color.red);
        CheckFlatForm();
    }

    private RaycastHit2D NearCastHit(List<RaycastHit2D> list) //분류된 RaycastHit2d를 플레이어와 거리가 가까운 순으로 정렬.
    {
        if (list.Count > 1)
        {
            list.Sort((x, y) =>
            (x.collider.transform.position - transform.position).sqrMagnitude.CompareTo((y.collider.transform.position - transform.position).sqrMagnitude));
        }
        else if (list.Count == 1)
        {
            //Debug.Log("리스트에 하나의 값만 있기 때문에 정렬할 필요가 없습니다.");
        }
        else
        {
            return default;
        }
        return list[0];
    }

    private void PlayerUIUpdate()
    {
        GameManager.instance.uIManager.combatUI.HpBarUpdate(maxHp, curHp);
        GameManager.instance.uIManager.combatUI.StmBarUpdate(maxStm, curStm);
        if (currentSkill != null)
        {
            GameManager.instance.uIManager.combatUI.CheckSkillCoolDown(currentSkill.RemainingCoolDown, currentSkill.coolDown);
        }
    }
    private void StatusInit()
    {
        //layerMask = 1 << PLATFORM_LAYER | 1 << INTERACTIVE_OBJECT_LAYER | 1 << CAN_CLIMB_WALL;

        level = 1;
        maxHp = 100;
        maxStm = 100;
        curHp = maxHp;
        curStm = maxStm;
        regenSpeed = 10f;
        curMoveSpeed = WALK_SPEED;
        jumpPower = 45;
        holdJumpPower = 20;
        att = 20;
        combo = 1;
        attCool = 2f;
        attTime = 0f;
        defense = 0;
        magicalDefense = 0;
        maxjumpHoldTime = 3f;
        coyoteTime = 0.2f;
        CanAction = true;
        canJump = true;
        damagabool = true;
        dir = new Vector3(1, 0).normalized;

        rigid = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
    }

    public void SetScale(int dir)
    {
        transform.localScale = new Vector3(scaleX * dir, scaleY, 1);
    }

    public Vector3 GetPosition()
    {
        return center.position;
    }
    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
    public int GetAttackPower()
    {
        return Att;
    }
    public IDamageable GetDamageableComponent()
    {
        return this;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetTag()
    {
        return this.gameObject.tag;
    }

    private void Climbing()
    {
        Debug.Log("벽에 붙어있는 중.");
        rigid.gravityScale = 0;
        moveY = Input.GetAxisRaw("Vertical");

        rigid.velocity = new Vector2(0, moveY * curMoveSpeed);
        //transform.localScale = new Vector2(scaleX, scaleY * dir.y);

        anim.CrossFade("Climbing", 0f);
    }

    private void CheckFlatForm() //플랫폼에 닿고 있는지 확인
    {
        if (checkingGround.collider != null) //플랫폼에 플레이어가 수직으로 닿고 있을 때.
        {
            if (checkingWall.collider != null) return;
            overground = false;
            CheckNormalVectorDown(checkingGround);
        }

        else //플랫폼에 발이 닿고 있지 않을 때.
        {
            overground = true;
            if (currentState != State.Climbing)
            {
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.SetParent(null);
                rigid.gravityScale = GRAVITY_SCALE;
                momentum = Vector2.zero;
                scaleX = 1;
                scaleY = 1;
            }
            else
            {
                rigid.gravityScale = 0;
            }
        }
    }

    private void CheckNormalVectorDown(RaycastHit2D hit) //플레이어가 지면에 닿아 있을 떄 실행하는 메서드
    {
        Debug.Log("수직으로 딛고 있음");
        IMovablePlatForm momentumPlatForm = hit.collider.GetComponent<IMovablePlatForm>() != null ? hit.collider.GetComponent<IMovablePlatForm>() : null;

        if (momentumPlatForm != null) //접촉한 플랫폼이 모멘텀 플랫폼인 경우.
        {
            transform.SetParent(hit.collider.transform);
            scaleX = (float)(1 / transform.parent.localScale.x);
            scaleY = (float)(1 / transform.parent.localScale.y);
            momentum = momentumPlatForm.GetMomentum();
        }
        else //접촉한 플랫폼이 모멘텀 플랫폼이 아닌 일반 플랫폼인 경우.
        {
            rigid.gravityScale = GRAVITY_SCALE;

            momentum = Vector2.zero;
            momentumX = momentum.x;
            momentumY = momentum.y;

            transform.SetParent(null);
            momentum = Vector2.zero;
            scaleX = 1;
            scaleY = 1;
        }

        currentState = State.Idle;
    }

    private void CheckNormalVectorSide(RaycastHit2D hit)
    {

    }

    private void InteractiveObject() //상호작용이 가능한 오브젝트에 닿고 있는지 확인.
    {
        RaycastHit2D hitObj;
        hitObj = NearCastHit(allRayCastHits);
        if (hitObj.collider == null || hitObj.collider.GetComponent<IInteractable>() == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("레버 상호작용");
            hitObj.collider.GetComponent<IInteractable>().Interact();
        }
    }

    private State StateUpdate()
    {
        float horizontal = moveX;
        bool checkAttack = attacking;
        bool _delayed = delayed;
        bool _climbing = checkingWall;

        if (rigid.velocity.y > 0 && overground && currentState != State.Climbing) { return State.Jumping; }

        else if (rigid.velocity.y < 0 && overground && currentState != State.Climbing)
        {
            if (currentState == State.Jumping) { return State.Jumping; }
            else { if (curcoyoteTime >= coyoteTime) { return State.Jumping; } }
        }

        return (horizontal, checkAttack, _delayed, _climbing) switch
        {
            (not 0, false, false, false) => State.Moving,
            (0, false, false, false) => State.Idle,
            (_, true, _, false) => State.Attacking,
            (_, _, true, false) => State.Attacking,
            (_, _, _, true) => State.Climbing,
        };
    }

    private void StateAction(State curState)
    {
        switch (curState)
        {
            case State.Idle:
                anim.CrossFade("Idle", 0f);
                attacking = false;
                Movement();
                break;

            case State.Moving:
                anim.CrossFade("Run", 0f);
                Movement();
                break;

            case State.Jumping:
                anim.CrossFade("Jump", 0f);
                Movement();
                break;

            case State.Climbing:
                //Movement();
                col.size = new Vector2(4, 2);
                Climbing();
                break;
        }
    }

    private void Movement()
    {
        if (moveX != 0)
        {
            //이동 중, 플레이어 진행 방향에 장애물이 존재하는지 확인.
            bool isHittingWall = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0, new Vector2(moveX, 0), 0.1f, 1 << PLATFORM_LAYER);
            if (isHittingWall)
            {
                moveX = 0;
            }
        }

        if (moveX != 0)// || moveY != 0)
        {
            dir = new Vector3(moveX, 0).normalized;
        }
        rigid.velocity = new Vector2((moveX + momentumX) * curMoveSpeed, rigid.velocity.y);
        transform.localScale = new Vector3(scaleX * dir.x, scaleY, 1);
    }

    private void ProccessCoyoteTime()
    {
        if (overground && rigid.velocity.y < 0 && currentState != State.Jumping && currentState != State.Climbing)
        {
            curcoyoteTime += Time.deltaTime;
        }
        else if (!overground && rigid.velocity.y >= 0)
        {
            curcoyoteTime = 0;
        }
    }

    private void Jump()
    {
        if (currentState == State.Climbing)
        {
            Debug.Log("벽타기 중 점프 누름");
            currentState = State.Jumping;
            rigid.gravityScale = GRAVITY_SCALE;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            return;
        }

        if (curcoyoteTime <= coyoteTime)
        {
            momentumX = momentum.x * .05f;
            momentumY = momentum.y;

            if (currentState != State.Jumping && !delayed && !attacking)
            {
                rigid.AddForce(Vector2.up * (jumpPower + momentumY), ForceMode2D.Impulse);
            }
        }
    }

    private void UseSkill() //스킬 사용메서드
    {
        int skillNum = (Input.inputString.ToUpper()) switch //이것도 스위치문.
        {
            ("C") => 1,
            ("V") => 2,
            ("B") => 3,
            _ => 0
        };

        if (skillNum == 0 || currentSkill == null) return;
        if (currentSkill.OnCoolDown) return;

        bool canUseSkill = false;

        if (!attacking && !delayed)
        {
            canUseSkill = true;
        }

        else if (delayed && currentSkill.CancleDelay)
        {
            canUseSkill = true;
        }

        if (canUseSkill)
        {
            if (currentSkill.UseSkill(this))
            {
                if (delayed && currentSkill.CancleDelay)
                {
                    attacking = false;
                    delayed = false;
                    hitBox.enabled = false;
                }
                currentState = State.Attacking;
                //anim.CrossFade("Skill_" + skillNum.ToString(), 0f);
                currentSkill.UseSkill(this);
                attacking = currentSkill.Attackable;
            }
        }
    }

    public T GetCom<T>() => this.GetComponent<T>();

    public BoxCollider2D GetHitBox()
    {
        return hitBox;
    }

    private void ProccessingPassive()
    {
        if (currentSkill.HavePassive)
        {
            currentSkill.ProccessPassive(this);
        }
    }

    private void InputAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !attacking)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        currentState = State.Attacking;
        anim.CrossFade("Attack_0" + combo, 0f);
        TotalDmg = att;
        attacking = true;
        delayed = false;
        if (combo == 1) combo++;
        else if (combo == 2) combo--;
        curStm -= 10;
        if (comboCoroutine != null) StopCoroutine(comboCoroutine);
    }

    public void Dead()
    {

    }

    public void Damaged(int dmg, string attackType)
    {
        if (damagabool)
        {
            Debug.Log("크아악!");
            Color txtcolor = new Color();
            int totalDmg = 0;
            damagabool = false;
            if (attackType == "Physical") { totalDmg = dmg - defense; txtcolor = Color.white; }
            else if (attackType == "Magical") { totalDmg = dmg - magicalDefense; txtcolor = Color.blue; }
            DamagedProcess(totalDmg, txtcolor);
            StartCoroutine(GraceTime());
        }
        else return;
    }

    IEnumerator GraceTime()
    {
        yield return gp;
        damagabool = true;
    }

    private void DamagedProcess(int totalDmg, Color txtColor)
    {
        //받은 데미지만큼 체력이 줄어듬
        curHp -= totalDmg;

        //UI에 받은 데미지 띄우기, 디버깅 용.
        var dmgText = GameManager.instance.objectPoolManger_DmgTxt.Pool.Get();
        dmgText.transform.SetParent(this.transform.GetChild(0));
        dmgText.transform.localPosition = new Vector2(0, 4.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
    }

    public void StatusEffectProcess(float duration, string effectName)
    {
        ApplyEffect(new Stun(duration, effectName, GetComponent<IDamageable>()));
    }

    private void StmRegen()
    {
        if (canRegen)
        {
            curStm += regenSpeed * Time.deltaTime;
        }
    }

    public void HitBoxOn()
    {
        hitBox.enabled = true;
    }

    public void HitBoxOff()
    {
        hitBox.enabled = false;
        attacking = false;
        delayed = true;
        comboCoroutine = StartCoroutine(ComboReset());
    }

    public void DelayEnd()
    {
        delayed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckGetDamage(other); //데미지를 주는 메서드.
    }

    private void CheckGetDamage(Collider2D other)
    {
        if (other.GetComponentInChildren<IDamageable>() != null && attacking)
        {
            IDamageable damagable = other.GetComponentInChildren<IDamageable>();
            damagable.Damaged(TotalDmg, "Physical");
            //damagable.StatusEffectProcess(5f, "Stun");
            GameManager.instance.InBattleState();
        }
    }

    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        if (!activeEffect.ContainsKey(effect.effectName))
        {
            activeEffect.Add(effect.effectName, effect);
            effect.ApplyEffect();
            GameManager.instance.uIManager.combatUI.CreateEffectUISlider(effect);
            GameManager.instance.uIManager.combatUI.UpdateEffectUI();
            newCorutine = StartCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines.Add(effect.effectName, newCorutine);
        }
        else
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                StopCoroutine(runningCoroutine);
                activeEffectCoroutines[effect.effectName] = StartCoroutine(RemoveEffectAfterDuration(effect));
                GameManager.instance.uIManager.combatUI.RenewalEffectSlider(effect, effect.effectName);
            }
        }
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect)
    {
        yield return new WaitForSeconds(effect.duration);
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
        effect.RemoveEffect();
        GameManager.instance.uIManager.combatUI.RemoveEffectUI(effect.effectName);
        GameManager.instance.uIManager.combatUI.UpdateEffectUI();
    }

    IEnumerator ComboReset()
    {
        yield return new WaitForSeconds(.2f);
        combo = 1;
    }

    public void RespawnInteract()
    {
        Debug.Log("리스폰 지점 저장");
        respawn = transform.position;
    }
}