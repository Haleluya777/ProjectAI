using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISkillCaster
{
    private enum State { Idle, Moving, Dash, Attacking, Jumping }
    public Vector3 respawn;
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    private RaycastHit2D raycastHit;
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private State currentState;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private GameObject particle;
    private int level;
    private int maxHp, curHp;
    private float maxStm, curStm;
    [SerializeField] private int curMoveSpeed;
    [SerializeField] private int jumpPower;
    private int holdJumpPower;
    private int scale = 1;
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
    public int CurrentHp => curHp;
    public int Att => att;
    public bool IsDead => isdead;
    public int Scale => scale;
    public bool CanAction { get; set; }
    private Vector3 dir;
    private float moveX;
    private float moveY;
    private float regenSpeed;
    private bool canRegen;
    [SerializeField] private bool delayed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float curcoyoteTime;
    [SerializeField] private float checkingDis;
    [SerializeField] private float attCool, attTime;
    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private Coroutine newCorutine;
    private Coroutine comboCoroutine;
    private const int WALK_SPEED = 15;
    public Vector3 Dir => dir;
    public Vector2 plusspeed;
    public bool overground;

    [SerializeField] private Skill_Module currentSkill;

    void Start()
    {
        StatusInit();
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
        CheckFlatForm();

        // 스킬 모듈의 쿨다운을 매 프레임 업데이트
        if (currentSkill != null)
        {
            currentSkill.UpdateCoolDown(Time.deltaTime);
        }

        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curStm = Mathf.Clamp(curStm, 0, maxStm);
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
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");

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
        dir = new Vector3(1, 0).normalized;
        rigid = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
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

    private void CheckFlatForm()
    {
        raycastHit = Physics2D.BoxCast(this.transform.position, new Vector2(1.5f, .5f), 0, this.transform.up * -1, .5f, layerMask);
        if (raycastHit.collider != null)
        {
            currentState = State.Idle;
            overground = false;
        }
        else
        {
            overground = true;
        }
    }

    private State StateUpdate()
    {
        float horizontal = moveX;
        bool checkAttack = attacking;
        bool _delayed = delayed;

        if (rigid.velocity.y > 0 && overground) { return State.Jumping; }

        else if (rigid.velocity.y < 0 && overground)
        {
            if (currentState == State.Jumping) { return State.Jumping; }
            else { if (curcoyoteTime >= coyoteTime) { return State.Jumping; } }
        }

        return (horizontal, checkAttack, _delayed) switch
        {
            (not 0, false, false) => State.Moving,
            (0, false, false) => State.Idle,
            (_, true, _) => State.Attacking,
            (_, _, true) => State.Attacking
        };
    }

    private void StateAction(State curState)
    {
        switch(curState)
        {
            case State.Idle:
                anim.CrossFade("Idle", 0f);
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
        }
    }

    private void Movement()
    {
        if (moveX != 0) 
        { 
            dir = new Vector3(moveX, 0).normalized;
        } 
        rigid.velocity = new Vector2 (moveX * curMoveSpeed, rigid.velocity.y); 
        transform.localScale = new Vector3(dir.x, 1, 1);
    }

    private void Jump()
    {
        if (overground && rigid.velocity.y < 0 && currentState != State.Jumping)
        {
            curcoyoteTime += Time.deltaTime;
        }
        else if(!overground && rigid.velocity.y >= 0)
        {
            curcoyoteTime = 0;
        }

        if (curcoyoteTime <= coyoteTime)
        {
            if (Input.GetButtonDown("Jump") && currentState != State.Jumping && !delayed && !attacking)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                curjumpHoldTime = 0;
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
        Color txtcolor = new Color();
        int totalDmg = 0;
        if (attackType == "Physical") { totalDmg = dmg - defense; txtcolor = Color.white; }
        else if (attackType == "Magical") { totalDmg = dmg - magicalDefense; txtcolor = Color.blue; }
        DamagedProcess(totalDmg, txtcolor);
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

        if (other.gameObject.layer == 6)
        {
            overground = false;
            currentState = State.Idle;
            attacking = false;
            plusspeed = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D ground)
    {
        
    }

    private void OnTriggerExit2D(Collider2D ground)
    {
        if (ground.gameObject.layer == 6)
        {
            overground = true;
            rigid.gravityScale = 12f;
            if (currentState != State.Jumping)
            {

            }
        }
    }

    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        if(!activeEffect.ContainsKey(effect.effectName))
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
        yield return new WaitForSeconds(1f);
        combo = 1;
    }

    public void RespawnInteract()
    {
        Debug.Log("리스폰 지점 저장");
        respawn = transform.position;
    }
}