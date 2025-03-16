using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Status : MonoBehaviour, IDamagable
{
    public enum EnemyAttackType { Physical, Magical }

    [SerializeField] private int maxHp, curHp;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int attack;
    [SerializeField] private int physicalDefense, magicalDefense;
    [SerializeField] private EnemyAttackType enemyAttackType;

    public void Initialize() //스테이터스 초기화화
    {
        maxHp = 100;
        curHp = maxHp;
        
        moveSpeed = 4;

        attack = 20;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<IDamagable>() != null)
        {
            other.GetComponent<IDamagable>().Damaged(attack, enemyAttackType.ToString());
        }
    }
}
