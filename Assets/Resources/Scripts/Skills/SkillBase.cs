using UnityEngine;


public abstract class SkillBase : ScriptableObject
{
    public enum DamageType{Physical, Magical}
    public DamageType damagType;
    public DmgCalculatorBase damageCalculator;
    public abstract bool UseSkill(ISkillCaster caster);
}