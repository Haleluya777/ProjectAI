using UnityEngine;


public abstract class SkillBase : ScriptableObject
{
    public string explanation;
    public Skill_Module parentModule { get; private set; }
    public enum DamageType { Physical, Magical }
    public DamageType damagType;
    public DmgCalculatorBase damageCalculator;
    public Vector2 HitBoxSize;
    public Vector2 HitBoxOffSet;

    public virtual void Initialize(Skill_Module module)
    {
        parentModule = module;
    }
    public abstract bool UseSkill(ISkillCaster caster);
}