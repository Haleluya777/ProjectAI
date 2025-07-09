using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, ISkillCaster
{
    // --- 상태 및 기본 변수들 (변경 없음) ---
    private enum State { Idle, Moving, Dash, Attacking, Jumping }
    public Vector3 respawn;
    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();
    [SerializeField] private GameObject statusEffectUI;
    [SerializeField] private State currentState;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private GameObject particle;
    private int level;
    private int maxHp, curHp;
    private float maxStm, curStm;
    private int curMoveSpeed;
    private int jumpPower;
    private int holdJumpPower;
    [SerializeField] private int combo;
    private bool isdead;
    private bool canJump;
    private float curjumpHoldTime;
    private float maxjumpHoldTime;
    [SerializeField] private int att, defense, magicalDefense;
    [SerializeField] private bool attacking;
    [SerializeField] private bool castingSkill;
    public int CurrentHp => curHp;
    public int Att => att;
    public bool IsDead => isdead;
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
    private const int WALK_SPEED = 10;
    public Vector3 Dir => dir;

    // --- 스킬 관련 변수 (새로운 설계) ---
    [SerializeField] private Skill_Module skillModule; // SkillBase 대신 Skill_Module을 직접 사용

    void Start()
    {
        StatusInit();
        // SetCaster 호출은 이제 필요 없음
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
        HandleSkillInput(); // UseSkill() 대신 새로운 이름의 메서드 호출
        StmRegen();
        PlayerUIUpdate();

        // 스킬 모듈의 쿨다운을 매 프레임 업데이트
        if (skillModule != null)
        {
            skillModule.UpdateCoolDown(Time.deltaTime);
        }

        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curStm = Mathf.Clamp(curStm, 0, maxStm);

        // --- 테스트용 코드 (변경 없음) ---
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyEffect(new Stun(5f, "Stun", gameObject.GetComponent<IDamageable>()));
        }
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

    private void PlayerUIUpdate() // UI 업데이트 메서드 수정
    {
        GameManager.instance.uIManager.combatUI.HpBarUpdate(maxHp, curHp);
        GameManager.instance.uIManager.combatUI.StmBarUpdate(maxStm, curStm);
        // Skill_Module의 쿨다운 정보를 UI에 전달
        if (skillModule != null)
        {
            GameManager.instance.uIManager.combatUI.CheckSkillCoolDown(skillModule.RemainingCoolDown, skillModule.coolDown);
        }
    }

    // --- 새로운 스킬 처리 메서드 ---
    private void HandleSkillInput()
    {
        // 'C' 키를 눌렀을 때 스킬 사용 시도
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (skillModule != null && !attacking && !delayed) // 공격 중이 아닐 때만 사용 가능 (필요에 따라 조건 변경)
            {
                if (skillModule.TryUseSkill(this)) // this는 ISkillCaster를 구현한 PlayerController 자신을 의미
                {
                    // 스킬 사용 성공 시 애니메이션 등 후처리
                    Debug.Log("스킬 사용 성공!");
                    currentState = State.Attacking; // 예시: 스킬 사용 시 공격 상태로 변경
                    anim.CrossFade("Skill_1", 0f); // 예시: 스킬 애니메이션 재생
                }
                else
                {
                    Debug.Log("스킬이 쿨다운 중입니다.");
                }
            }
        }
    }

    // --- 기존 메서드들 (대부분 변경 없음) ---
    private void StatusInit()
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

    private State StateUpdate()
    {
        float horizontal = moveX;
        bool checkAttack = attacking;
        bool _delayed = delayed;
        if (rigid.velocity.y > 0) { return State.Jumping; }
        else if (rigid.velocity.y < 0)
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
            case State.Idle: anim.CrossFade("Idle", 0f); break;
            case State.Moving: anim.CrossFade("Run", 0f); Movement(); break;
            case State.Jumping: anim.CrossFade("Jump", 0f); Movement(); break;
        }
    }

    private void Movement()
    {
        dir = new Vector3(moveX, 0).normalized;
        if (moveX != 0) { transform.localScale = new Vector3(dir.x, 1, 1); }
        transform.position += dir * curMoveSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        if (rigid.velocity.y < 0 && currentState != State.Jumping) { curcoyoteTime += Time.deltaTime; }
        else if(rigid.velocity.y == 0 || rigid.velocity.y > 0) { curcoyoteTime = 0; }

        if (curcoyoteTime <= coyoteTime)
        {
            if (Input.GetButtonDown("Jump") && currentState != State.Jumping && !delayed && !attacking)
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
        if (other.gameObject.layer == 6) { currentState = State.Idle; attacking = false; }
    }

    private void ApplyEffect(StatusEffect effect)
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