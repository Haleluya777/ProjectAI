using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour//, IDamagable
{
    public enum EnemyAttackType { Physical, Magical }

    [SerializeField] private int maxHp, curHp;
    [SerializeField] private int moveSpeed; //이동 스크립트에서 참조해야 함.
    [SerializeField] private int attack; //공격 스크립트에서 참조해야 함.
    [SerializeField] private float meleeDistance; //공격 스크립트에서 참조해야 함.
    [SerializeField] private float rangeDistance; //이하동문

    [SerializeField] private int physicalDefense, magicalDefense;
    [SerializeField] private EnemyAttackType enemyAttackType; //공격 스크립트에서 참조해야 함

    public int Attack => attack;
    public int MoveSpeed => moveSpeed;
    public EnemyAttackType AttackType => enemyAttackType;

    public void Initialize() //스테이터스 초기화화
    {
        maxHp = 100;
        curHp = maxHp;
        
        moveSpeed = 4;

        attack = 20;
        meleeDistance = 3f;
        rangeDistance = 6f;

        physicalDefense = 10;
        magicalDefense = 5;
    }
    
    public void Damaged(int dmg, string attackType)
    {
        Color txtcolor = new Color();
        int totalDmg = new int();

        if(attackType == "Physical")
        {
            totalDmg = dmg - physicalDefense;
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

        if(curHp <= 0)
        {
            //사망 처리
        }
    }

    public void StatusEffectProcess(float duration, string effectName)
    {
        //ApplyEffect(new Stun(duration, effectName, GetComponent<PlayerController>()));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().Damaged(attack, enemyAttackType.ToString());
        }
    }
}
