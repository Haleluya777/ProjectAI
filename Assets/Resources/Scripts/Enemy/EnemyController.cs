using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    //PlayerController랑 별 차이도 없음.
    //EnemyModuleScript폴더 내 스크립트들은 아직 미완.
    
    private enum State { Idle, Guarding, Tracking, Attack }
    public enum EnemyAttackType { Physical, Magical }

    private Dictionary<string, StatusEffect> activeEffect = new Dictionary<string, StatusEffect>();
    private Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>();

    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private BoxCollider2D hitBox;

    [SerializeField] private State currentState;

    private int maxHp, curHp;
    private int att, defense, magicalDefense;
    private int scale = 2;
    private float attRange;
    private float detectionRange;
    private float boundaryRange;
    private float moveSpeed;
    private bool isdead;


    private GameObject target;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 moveDir;
    public EnemyAttackType enemyAttackType;
    private Coroutine newCorutine;

    private int dir;
    private float distance;
    private float attTime, attCool;
    private bool canAttack;
    private bool targeting;

    //-------------Property-------------//
    public bool CanAction { get; set; }
    public int CurrentHp => curHp;
    public bool IsDead => isdead;
    public int Scale => scale;
    //----------------------------------//


    // Start is called before the first frame update
    void Start()
    {
        target = GameManager.instance.playerObj;

        StatusInit();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Enemy 적용중인 상태 이상 : {activeEffect.Count}");

        attTime += Time.deltaTime;
        distance = Mathf.Abs(this.transform.position.x - target.transform.position.x);
        targeting = currentState != State.Idle ? true : false;
        dir = target.transform.position.x < this.transform.position.x ? 1 : -1;
        currentState = StateUpdate(distance);

        StateAction(currentState);
        Targeting(dir);

        enemyUI.HpBarUpdate(maxHp, curHp);
        enemyUI.TextUpdate(currentState.ToString());
    }

    private void StatusInit()
    {
        maxHp = 100;
        curHp = maxHp;

        att = 20;
        defense = 3;
        magicalDefense = 5;
        attCool = 2f;
        attTime = 0f;
        attRange = 2f;
        
        moveSpeed = 2f;
        detectionRange = 10f;
        boundaryRange = 12f;

        currentState = State.Idle;
        anim = this.GetComponent<Animator>();
        sprite = this.GetComponent <SpriteRenderer>();
    }

    //현재 상태에 따른 행동 규정정
    private void StateAction(State curState)
    {
        switch(curState)
        {
            case State.Idle:
                anim.SetBool("isMoving", false);
                break;

            case State.Guarding:
                anim.SetBool("isMoving", false);
                break;

            case State.Tracking:
                Movement();
                break;

            case State.Attack:
                AttackTime();
                anim.SetBool("isMoving", false);
                break;
        }
    }

    //거리에 따라 현재 상태 변경
    private State StateUpdate(float distance) => distance switch
    {
        _ when distance <= boundaryRange && distance > detectionRange => State.Guarding,
        _ when distance <= detectionRange && distance > attRange => State.Tracking,
        _ when distance <= attRange && canAttack => State.Attack,

        _ => State.Idle
        
    };

    private void Targeting(int dir)
    {
        if(targeting)
        {
            transform.localScale = new Vector3(dir, 1, 1);
        }
    }

    private void Movement()
    {
        moveDir = (Vector2.left * dir).normalized;
        distance = Mathf.Abs(this.transform.position.x - target.transform.position.x);

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
        anim.SetBool("isMoving", true);
    }

    private void AttackTime()
    {
        if(attTime >= attCool && distance <= attRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        attTime = 0f;
    }

    public void Dead()
    {

    }

    public void Damaged(int dmg, string attackType)
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

    private void DamagedProcess(int totalDmg, Color txtColor)
    {
        curHp -= totalDmg;

        var dmgText = GameManager.instance.objectPoolManger_DmgTxt.Pool.Get();
        dmgText.transform.parent = this.transform.GetChild(0);
        dmgText.transform.localPosition = new Vector2(0, 5.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
    }

    public void StatusEffectProcess(float duration, string effectName)
    {
        ApplyEffect(new Stun(duration, effectName, GetComponent<EnemyController>()));
    }

    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        if(!activeEffect.ContainsKey(effect.effectName)) //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있지 않을 경우.
        {
            activeEffect.Add(effect.effectName, effect);
            effect.ApplyEffect();
            newCorutine = StartCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines.Add(effect.effectName, newCorutine);
        }

        else //적용하려는 상태 이상이 현재 플레이어에게 작용하고 있는 경우.
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                StopCoroutine(runningCoroutine);
                activeEffectCoroutines[effect.effectName] = StartCoroutine(RemoveEffectAfterDuration(effect));
            }
        }
    }

    public void HitBoxOn()
    {
        hitBox.enabled = true;
    }

    public void HitBoxOff()
    {
        hitBox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            IDamageable damagable =  other.GetComponent<IDamageable>();
            damagable.Damaged(att, enemyAttackType.ToString());
            damagable.StatusEffectProcess(.5f, "Stun");
            GameManager.instance.InBattleState();
        }
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect) //상태 이상 제거.
    {
        yield return new WaitForSeconds(effect.duration);
        effect.RemoveEffect();
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
    }
}
