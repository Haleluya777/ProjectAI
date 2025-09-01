using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Skill/DmgCalculatorTest")]
public class DmgCalculatorTest : DmgCalculatorBase
{
    public int baseDmg;
    public float dmgWeight;

    public override int CalculateDmg(ISkillCaster attacker)
    {
        return baseDmg + (int)(attacker.GetAttackPower() * dmgWeight);
    }
}
