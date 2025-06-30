using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISkillCaster
{
    private enum State { Idle, Moving, Dash, Attacking, Jumping } //현재 플레이어 상태.
    public Vector3 respawn;

    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();

    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private State currentState;
    [SerializeField] private SkillBase currentSkill;
    //[SerializeField] private List<SkillBase> currentSkill = new List<SkillBase>(); //나중에 쓸 리스트
    [SerializeField] private int skillNum;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private GameObject particle;

    private int level; //Player Level

    private int maxHp, curHp;
    private float maxStm, curStm;
    private int curMoveSpeed; //Current Move Speed
    private int jumpPower;
    private int holdJumpPower;
    private bool isdead;
    private bool canJump;
    private float curjumpHoldTime;
    private float maxjumpHoldTime;
    private int layerMask;

    [SerializeField] private int att, defense, magicalDefense;
    [SerializeField] private bool attacking;
    //-------------Property-------------//
    public int CurrentHp => curHp;
    public int Att => att;
    public bool IsDead => isdead;
    public bool CanAction { get; set; }
    //----------------------------------//

    private Vector3 dir;
    private float moveX;
    private float moveY;
    private float regenSpeed;
    private float remainingCoolDown;
    private string inputKey;
    private bool canRegen;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float curcoyoteTime;
    [SerializeField] private float checkingDis;
    [SerializeField] private float attCool, attTime;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Coroutine newCorutine;

    private const int WALK_SPEED = 10;

    public Vector3 Dir => dir;

    void Start()
    {
        StatusInit();
        if (currentSkill.Caster == null)
        {
            currentSkill.SetCaster(this);
        }
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");
    }

    // Update is called once per frame
    void Update()
    {
        //매 프레임당 확인할 요소들
        //사실 매 프레임당 메서드를 실행시키는 건 메모리 저하를 일으킬 가능성이 높음.
        //그러나 2D 인디 게임 특성상 사용하는 메모리가 그리 많지 않기 때문에 Update문에 몰아서 사용.
        //보통은 키보드 입력을 제외한 나머지 요소들은 Fixed업데이트에 넣거나 필요할 때만 호출하도록 함.

        //Debug.Log($"현재 적용된 상태 이상 개수 : {activeEffect.Count}");

        moveX = Input.GetAxisRaw("Horizontal");
        if (currentState == State.Idle) { canRegen = true; } else { canRegen = false; }
        
        Jump();

        if (CanAction)
        {
            currentState = StateUpdate();
            StateAction(currentState);
        }
        
        BasicAttackTime();
        StmRegen();
        PlayerUIUpdate();
        UseSkill();

        currentSkill.UpdateCoolDown(Time.deltaTime);

        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curStm = Mathf.Clamp(curStm, 0, maxStm);


        //상태이상이 잘 들어가나 확인용 테스트 코드.
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //상태이상을 제공하는 제공자가 사용할 메서드.
            //현재는 ApplyEffect를 이용해서 상태이상을 제공하지 않음.
            //StatusEffectProces 사용할 것.
            ApplyEffect(new Stun(5f, "Stun", gameObject.GetComponent<IDamageable>()));
        }

        //이하 테스트용
        if (Input.GetKeyDown(KeyCode.W))
        {
            ApplyEffect(new Stun(5f, "DontMove", gameObject.GetComponent<IDamageable>()));
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            ApplyEffect(new Stun(5f, "Haleluya", gameObject.GetComponent<IDamageable>()));
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.InBattleState();
        }
    }

    private void FixedUpdate()
    {
        //CheckingPlatForm();
    }

    private void PlayerUIUpdate() //플레이어 UI상태를 업데이트하는 메서드
    {
        GameManager.instance.uIManager.combatUI.HpBarUpdate(maxHp, curHp);
        GameManager.instance.uIManager.combatUI.StmBarUpdate(maxStm, curStm);
        GameManager.instance.uIManager.combatUI.CheckSkillCoolDown(currentSkill.RemainingCoolDown, currentSkill.coolDown);
    }

    private void StatusInit() //Status Initialize
    {
        level = 1;

        maxHp = 100;
        maxStm = 100;
        curHp = maxHp;
        curStm = maxStm;
        regenSpeed = 10f;

        curMoveSpeed = WALK_SPEED;
        jumpPower = 15;
        holdJumpPower = 20;
        att = 20;
        attCool = 2f;
        attTime = 0f;
        defense = 0;
        magicalDefense = 0;
        remainingCoolDown = 0f;
        curjumpHoldTime = 0f;
        maxjumpHoldTime = 3f;
        coyoteTime = 0.2f;

        CanAction = true;
        canJump = true;

        rigid = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
    }

    //ISkillCaster 메서드 재정의
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
        return this; // PlayerController가 IDamageable을 구현하므로 자신을 반환
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    // 여기까지 ISkillCaster재정의

    private State StateUpdate() //플레이어의 상태를 반환함.
    {
        float horizontal = moveX;
        bool checkAttack = attacking;
        if (rigid.velocity.y > 0) { return State.Jumping; }
        else if (rigid.velocity.y < 0)
        {
            if (currentState == State.Jumping)
            {
                return State.Jumping;
            }
            else
            {
                if (curcoyoteTime >= coyoteTime)
                {
                    return State.Jumping;
                }
            } 
        }

        return (horizontal, checkAttack) switch //이거 스위치문.
        {
            (not 0, false) => State.Moving,
            (0, false) => State.Idle,
            (_, true) => State.Attacking
        };
    }

    private void StateAction(State curState)
    {
        switch(curState)
        {
            case State.Idle:
                anim.SetBool("isMoving", false);
                anim.SetBool("IsJumping", false);
                break;

            case State.Moving:
                anim.SetBool("isMoving", true);
                anim.SetBool("IsJumping", false);
                Movement();
                break;

            case State.Jumping:
                anim.SetBool("IsJumping", true);
                Movement();
                break;
        }
    }

    private void Movement() //이동 메서드. 입력값을 받아서 작동하는 건 아님.
    {
        dir = new Vector3(moveX, 0).normalized;
        if (moveX == 0) { } else { transform.localScale = new Vector3(dir.x, 1, 1); }
        transform.position += dir * curMoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (rigid.velocity.y < 0 && currentState != State.Jumping)
        {
            curcoyoteTime += Time.deltaTime;
        }
        else if(rigid.velocity.y == 0 || rigid.velocity.y > 0)
        {
            curcoyoteTime = 0;
        }

        if (curcoyoteTime <= coyoteTime)
        {
            if (Input.GetButtonDown("Jump") && currentState != State.Jumping)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curjumpHoldTime = 0;
            }

            if (Input.GetButton("Jump") && currentState == State.Jumping)
            {
                if (curjumpHoldTime < maxjumpHoldTime)
                {
                    rigid.velocity += new Vector2(0, holdJumpPower * Time.deltaTime);
                    curjumpHoldTime += Time.deltaTime;
                }
            }
        }
    }

    private void CheckingPlatForm()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkingDis, layerMask);
        Debug.DrawRay(transform.position, Vector2.down * checkingDis, Color.red);
        Debug.Log(hit.collider == null);
        currentState = (hit.collider != null) ? State.Idle : State.Jumping;
    }

    private void BasicAttackTime() //기본 공격 쿨타임.
    {
        attTime += Time.deltaTime;
        if (attTime >= attCool && Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
    }

    private void Attack()
    {
        currentState = State.Attacking;
        anim.SetTrigger("isAttack");
        attacking = true;
        attTime = 0f;
        curStm -= 10;
    }

    private void UseSkill() //스킬 사용메서드
    {
        skillNum = (Input.inputString.ToUpper()) switch //이것도 스위치문.
        {
            ("C") => 1,
            ("V") => 2,
            ("B") => 3,
            _ => 0
        };
        
        if(skillNum != 0 && !currentSkill.OnCoolDown)
        {
            //Debug.Log("스킬 사용!");
            currentState = State.Attacking;
            attacking = true;
            anim.SetTrigger("Skill_" + skillNum.ToString());
            currentSkill.UseSkill();
        }
    }

    public void Dead()
    {
        
    }

    public void Damaged(int dmg, string attackType) //데미지를 받을 때 실행시킬 메서드. Damagable인터페이스를 상속했기 때문에 무조건 이 메서드는 정의되어야 함.
    {
        Color txtcolor = new Color();
        int totalDmg = new int();

        if (attackType == "Physical")
        {
            totalDmg = dmg - defense;
            txtcolor = Color.white;
        }

        else if (attackType == "Magical")
        {
            totalDmg = dmg - magicalDefense;
            txtcolor = Color.blue;
        }

        DamagedProcess(totalDmg, txtcolor);
    }

    private void DamagedProcess(int totalDmg, Color txtColor) //데미지 받는 과정.
    {
        curHp -= totalDmg;

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
        if(canRegen)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInChildren<IDamageable>() != null && attacking) //충돌한 오브젝트가 IDamagable인터페이스를 상속하면 아래 명령어 실행.
        {
            IDamageable damagable = other.GetComponentInChildren<IDamageable>();
            damagable.Damaged(att, "Physical");
            damagable.StatusEffectProcess(5f, "Stun");
            GameManager.instance.InBattleState();
        }

        if (other.gameObject.layer == 6)
        {
            Debug.Log("땅에 닿음");
            currentState = State.Idle;
            attacking = false;
        }
    }

    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        if(!activeEffect.ContainsKey(effect.effectName)) //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있지 않을 경우.
        {
            activeEffect.Add(effect.effectName, effect);
            effect.ApplyEffect();
            GameManager.instance.uIManager.combatUI.CreateEffectUISlider(effect);
            GameManager.instance.uIManager.combatUI.UpdateEffectUI();
            newCorutine = StartCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines.Add(effect.effectName, newCorutine);
        }

        else //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있는 경우.
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                StopCoroutine(runningCoroutine);
                activeEffectCoroutines[effect.effectName] = StartCoroutine(RemoveEffectAfterDuration(effect));
                GameManager.instance.uIManager.combatUI.RenewalEffectSlider(effect, effect.effectName);
            }
        }
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect) //상태 이상 제거.
    {
        yield return new WaitForSeconds(effect.duration);
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
        effect.RemoveEffect();
        GameManager.instance.uIManager.combatUI.RemoveEffectUI(effect.effectName);
        GameManager.instance.uIManager.combatUI.UpdateEffectUI();
    }

    public void RespawnInteract()
    {
        Debug.Log("리스폰 지점 저장");
        respawn = transform.position;
    }
}
