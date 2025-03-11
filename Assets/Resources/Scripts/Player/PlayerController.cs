using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    private enum State { Idle, Moving, Dash, Attacking }
    [SerializeField] private List<StatusEffect> activeEffect = new List<StatusEffect>();

    [SerializeField] private State currentState;
    [SerializeField] private SkillBase currentSkill;
    [SerializeField] private int skillNum;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private GameObject particle;

    [SerializeField] private int level; //Player Level

    [SerializeField] private float maxHp, curHp, maxStm, curStm;
    [SerializeField] private int curMoveSpeed; //Current Move Speed
    [SerializeField] private int jumpPower;
    [SerializeField] private int att, defense, magicalDefense;
    [SerializeField] private bool attacking;
    [SerializeField] public bool canAction;
    //-------------Property-------------//
    public int Att => att;
    //----------------------------------//

    private Vector3 dir;
    private float moveX;
    private float regenSpeed;
    private string inputKey;
    private bool canRegen;
    [SerializeField] private float attCool, attTime;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    private WaitForSeconds dashTime = new WaitForSeconds(.3f);

    private const int WALK_SPEED = 5;
    private const int RUN_SPEED = 7;
    private const int DASH_SPEED = 10;

    public Vector3 Dir => dir;

    void Start()
    {
        StatusInit();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        currentState = StateUpdate();
        canRegen = currentState == State.Idle ? true : false;

        StateAction(currentState);
        BasicAttackTime();
        StmRegen();
        RunningStm();
        PlayerUIUpdate();
        UseSkill();
        currentSkill.UpdateCoolDown(Time.deltaTime);

        curHp = Mathf.Clamp(curHp, 0, maxHp);
        curStm = Mathf.Clamp(curStm, 0, maxStm);

        if(Input.GetKeyDown(KeyCode.Q))
        {
            ApplyEffect(new Stun(.3f, gameObject.GetComponent<PlayerController>()));
        }
    }

    private void PlayerUIUpdate()
    {
        playerUI.HpBarUpdate(maxHp, curHp);
        playerUI.StmBarUpdate(maxStm, curStm);
        playerUI.TextUpdate(currentState.ToString());
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
        jumpPower = 10;
        att = 10;
        attCool = 2f;
        attTime = 0f;
        defense = 0;
        magicalDefense = 0;

        canAction = true;

        rigid = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent <Animator>();
        sprite = this.GetComponent<SpriteRenderer>();

        currentSkill.player = this.gameObject.GetComponent<PlayerController>();
    }

    private State StateUpdate()
    {
        float horizontal = moveX;
        bool checkAttack = attacking;

        return (horizontal, checkAttack) switch
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
                break;

            case State.Moving:
                Movement();
                break;
        }
    }

    private void Movement() 
    {
        dir = new Vector3(moveX, 0).normalized;

        anim.SetBool("isMoving", true);
        transform.localScale = new Vector3(dir.x, 1, 1);

        transform.position += dir * curMoveSpeed * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.X))
        {
            Dash();
        }

        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            curMoveSpeed = RUN_SPEED;
        }

        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            curMoveSpeed = WALK_SPEED;
        }
    }

    private void RunningStm()
    {
        if(curMoveSpeed == RUN_SPEED)
        {
            curStm -= 2f * Time.deltaTime;
        }
    }

    private void FastRun()
    {
        if(currentState == State.Moving && Input.GetKeyDown(KeyCode.LeftShift))
        {
            curStm -= 2f * Time.deltaTime;
            curMoveSpeed = RUN_SPEED;
        }
        else
        {
            curMoveSpeed = WALK_SPEED;
        }
    }

    private void Dash()
    {
        Debug.Log("Dash!");
        particle.SetActive(true);
        curMoveSpeed = DASH_SPEED;
        curStm -= 20f;
        StartCoroutine("SpeedReturn");
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && rigid.velocity.y == 0 && curStm > 10)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            curStm -= 10;
        }
    }

    private void BasicAttackTime()
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

    private void UseSkill()
    {
        skillNum = (Input.inputString.ToUpper()) switch
        {
            ("C") => 1,
            ("V") => 2,
            ("B") => 3,
            _ => 0
        };
        
        if(skillNum != 0 && !currentSkill.OnCoolDown)
        {
            Debug.Log("스킬 사용!");
            currentState = State.Attacking;
            attacking = true;
            anim.SetTrigger("Skill_" + skillNum.ToString());
            currentSkill.UseSkill();
        }
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
        dmgText.transform.localPosition = new Vector2(0, 4.5f);
        dmgText.GetComponent<DmgText>().SetDmgText(totalDmg, txtColor);
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
        if (other.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().Damaged(att, "Physical");
        }
    }


    private void ApplyEffect(StatusEffect effect) //상태 이상 적용.
    {
        effect.ApplyEffect();
        activeEffect.Add(effect);
        StartCoroutine(RemoveEffectAfterDuration(effect));
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffect effect) //상태 이상 제거.
    {
        yield return new WaitForSeconds(effect.duration);
        effect.RemoveEffect();
        activeEffect.Remove(effect);
    }

    IEnumerator SpeedReturn()
    {
        yield return dashTime;
        particle.SetActive(false);
        curMoveSpeed = WALK_SPEED;
    }
}
