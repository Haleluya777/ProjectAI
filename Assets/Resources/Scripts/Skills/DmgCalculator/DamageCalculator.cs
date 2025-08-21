using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageCalculator : DamageCalculatorBase
{
    [SerializeField] private string calculatorName;
    [SerializeField] private int baseDmg;
    [SerializeField] private float attackRatio;

    public string Calculatorname => calculatorName;
    public int BaseDmg => baseDmg;
    public float AttackRatio => attackRatio;

    //public override int CalculateDamage(Unit attacker)
    //{
    //    return baseDmg + (int)(attacker.Attack *  attackRatio);
    //}
}
