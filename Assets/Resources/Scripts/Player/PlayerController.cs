using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISkillCaster
{
    private enum State { Idle, Moving, Dash, Attacking, Jumping, Climbing }
    public Vector3 respawn;
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    [SerializeField] private Transform center;

    //레이캐스트 설정----
    private RaycastHit2D checkingWall;
    private List<RaycastHit2D> allRayCastHits = new List<RaycastHit2D>();
    private int hitCount;
    private ContactFilter2D contactFilter; //레이어, isTrigger필터
    private List<RaycastHit2D> platformHits = new List<RaycastHit2D>();
    private List<RaycastHit2D> interactHits = new List<RaycastHit2D>();

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
    [SerializeField] private float scale = 1;
    private int layerMask;
    private int combo;
    private bool isdead;
    private bool canJump;
    private bool canDamaged;
    private float curjumpHoldTime;
    private float maxjumpHoldTime;
    [SerializeField] private int att, defense, magicalDefense;
    [SerializeField] private bool attacking;
    [SerializeField] private bool castingSkill;
    //

    public int CurrentHp => curHp;
    public int Att => att;
    public bool IsDead => isdead;
    public float Scale => scale;
    public bool CanAction { get; set; }
    private Vector3 dir;
    private float moveX;
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
    private new WaitForSeconds gp;
    [SerializeField] private bool damagabool;
    [SerializeField] private Skill_Module currentSkill;

    void Start()
    {
        StatusInit();

        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layerMask);
        contactFilter.useTriggers = true;

        gp = new WaitForSeconds(gracePeriod);
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        if (currentState == State.Idle) { canRegen = true; } else { canRegen = false; }

        Jump();

        if (CanAction)
        {
            currentState = StateUpdate();
            StateAction(currentState);
        }

        InputAttack();
        UseSkill();
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
    }

    private void FixedUpdate()
    {
        platformHits.Clear();
        interactHits.Clear();

        checkingWall = Physics2D.Raycast(center.position, Vector2.right, 1f, 1 << CAN_CLIMB_WALL);
        hitCount = Physics2D.BoxCast(col.bounds.center, new Vector2(col.bounds.size.x + .1f, col.bounds.size.y + .1f), 0, Vector2.zero, contactFilter, allRayCastHits, 0f);

        if (hitCount == 0)
        {
            CheckFlatForm();
            return;
        }

        for (int i = 0; i < hitCount; i++) //레이캐스트에 접촉한 모든 RaycastHit2d를 충돌한 오브젝트의 레이어에 맞게 분류하는 작업.
        {
            RaycastHit2D currentHit = allRayCastHits[i];
            int currentLayer = currentHit.collider.gameObject.layer;

            if (currentLayer == PLATFORM_LAYER)
            {
                platformHits.Add(currentHit);
            }
            else if (currentLayer == INTERACTIVE_OBJECT_LAYER)
            {
                interactHits.Add(currentHit);
            }
        }

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
            Debug.Log("리스트에 하나의 값만 있기 때문에 정렬할 필요가 없습니다.");
        }
        else
        {
            Debug.Log("박스캐스트에 감지된 오브젝트가 존재하지 않습니다.");
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
        layerMask = 1 << PLATFORM_LAYER | 1 << INTERACTIVE_OBJECT_LAYER;

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
        curjumpHoldTime = 0f;
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

    public Vector3 GetPosition()
    {
        return transform.position;
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
        rigid.gravityScale = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //rigid.velocity = new Vector2(0, 1 * curMoveSpeed);
        }
        else
        {

        }
        rigid.velocity = Vector2.zero;
        Debug.Log("벽에 붙어있는 중.");
    }

    private void CheckFlatForm() //플랫폼에 닿고 있는지 확인
    {
        RaycastHit2D hitPlatform = NearCastHit(platformHits); //접촉한 플랫폼 중 가장 가까운 플랫폼을 저장.
        if (hitPlatform.collider != null) //접촉한 플랫폼이 존재할 경우.
        {
            if (Vector2.Dot(hitPlatform.normal, Vector2.up) > .9f)
            {
                Debug.Log("아야야");
                IMovablePlatForm momentumPlatForm = hitPlatform.collider.GetComponent<IMovablePlatForm>() != null ? hitPlatform.collider.GetComponent<IMovablePlatForm>() : null;

                if (momentumPlatForm != null) //접촉한 플랫폼이 모멘텀 플랫폼인 경우.
                {
                    transform.SetParent(hitPlatform.collider.transform);
                    scale = (float)(1 / transform.parent.localScale.x);
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
                    scale = 1;
                }

                currentState = State.Idle;
                overground = false;
            }
        }
        else //접촉한 플랫폼이 없는 경우 (공중에 있을 때.)
        {
            transform.SetParent(null);
            rigid.gravityScale = GRAVITY_SCALE;
            momentum = Vector2.zero;
            scale = 1;
            overground = true;
        }
    }

    private void InteractiveObject() //상호작용이 가능한 오브젝트에 닿고 있는지 확인.
    {
        RaycastHit2D hitObj;
        hitObj = NearCastHit(interactHits);
        if (hitObj.collider == null || hitObj.collider.GetComponent<IInteractable>() == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("레버 상호작용");
            hitObj.collider.GetComponent<IInteractable>().Interacte();
        }
    }

    private State StateUpdate()
    {
        float horizontal = moveX;
        bool checkAttack = attacking;
        bool _delayed = delayed;
        bool _climbing = checkingWall;

        if (rigid.velocity.y > 0 && overground) { return State.Jumping; }

        else if (rigid.velocity.y < 0 && overground)
        {
            if (currentState == State.Jumping) { return State.Jumping; }
            else { if (curcoyoteTime >= coyoteTime) { return State.Jumping; } }
        }

        if (currentState != State.Jumping)
        {
            return (horizontal, checkAttack, _delayed, _climbing) switch
            {
                (not 0, false, false, false) => State.Moving,
                (0, false, false, false) => State.Idle,
                (_, true, _, false) => State.Attacking,
                (_, _, true, false) => State.Attacking,
                (_, _, _, true) => State.Climbing,
            };
        }
        else
        {
            return State.Jumping;
        }
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
                Climbing();
                break;
        }
    }

    private void Movement()
    {
        if (moveX != 0)
        {
            dir = new Vector3(moveX, 0).normalized;
        }
        rigid.velocity = new Vector2((moveX + momentumX) * curMoveSpeed, rigid.velocity.y);
        transform.localScale = new Vector3(scale * dir.x, scale, scale);
    }

    private void Jump()
    {
        if (overground && rigid.velocity.y < 0 && currentState != State.Jumping)
        {
            curcoyoteTime += Time.deltaTime;
        }
        else if (!overground && rigid.velocity.y >= 0)
        {
            curcoyoteTime = 0;
        }

        if (curcoyoteTime <= coyoteTime)
        {
            if (Input.GetButtonDown("Jump"))
            {
                momentumX = momentum.x * .05f;
                momentumY = momentum.y;

                if (currentState == State.Climbing)
                {
                    Debug.Log("벽타기 중 점프 누름");
                }

                else if (currentState != State.Jumping && !delayed && !attacking)
                {
                    rigid.AddForce(Vector2.up * (jumpPower + momentumY), ForceMode2D.Impulse);
                    curjumpHoldTime = 0;
                }
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
                anim.CrossFade("Skill_" + skillNum.ToString(), 0f);
                currentSkill.UseSkill(this);
                attacking = currentSkill.Attackable;
            }
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
        if (other.GetComponentInChildren<IDamageable>() != null && attacking)
        {
            IDamageable damagable = other.GetComponentInChildren<IDamageable>();
            damagable.Damaged(att, "Physical");
            damagable.StatusEffectProcess(5f, "Stun");
            GameManager.instance.InBattleState();
        }
    }

    private void OnTriggerExit2D(Collider2D ground)
    {
        if (ground.gameObject.layer == 6)
        {
            //overground = true;
            //rigid.gravityScale = 12f;
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