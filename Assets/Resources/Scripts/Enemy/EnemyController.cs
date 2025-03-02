using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    private enum State { Idle, Guarding, Tracking, Attack }
    public enum EnemyAttackType { Physical, Magical }

    [SerializeField] private EnemyUI enemyUI;
    [SerializeField] private BoxCollider2D hitBox;

    [SerializeField] private State currentState;

    private int maxHp, curHp;
    private int att, defense, magicalDefense;

    private float attRange;
    private float detectionRange;
    private float boundaryRange;
    private float moveSpeed;

    private GameObject target;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 moveDir;
    public EnemyAttackType enemyAttackType;

    private int dir;
    private float distance;
    private float attTime, attCool;
    private bool canAttack;
    private bool targeting;

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

    //��ġ�� ���� ���°� ��ȭ
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

    public void Damaged(int dmg, string attackType)
    {
        Color txtcolor = new Color();
        int totalDmg = new int();

        if(attackType == "Physical")
        {
            totalDmg = dmg - defense;
            txtcolor = Color.white;
        }

        else if(attackType == "Magical")
        {
            totalDmg = dmg - magicalDefense;
            txtcolor = Color.blue;
        }

        DamagedProcess(totalDmg, txtcolor);
    }

    private void DamagedProcess(int totalDmg, Color txtColor)
    {
        curHp -= totalDmg;

        var dmgText = GameManager.instance.objectPoolManger.Pool.Get();
        dmgText.transform.parent = this.transform.GetChild(0);
        dmgText.transform.localPosition = new Vector2(0, 5.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
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
        if(other.GetComponent<IDamagable>() != null)
        {
            Debug.Log("우히히");
            other.GetComponent<IDamagable>().Damaged(att, enemyAttackType.ToString());
        }
    }
}
