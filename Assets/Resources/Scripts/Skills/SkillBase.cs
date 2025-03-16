using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/SkillBase")]
public abstract class SkillBase : ScriptableObject
{
    public enum AttackType { Physical, Magical }

    public DmgCalculatorBase damageCalculator;
    public AttackType attackType;
    public PlayerController player;
    public float coolDown;
    private float remainingCoolDown;

    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;

    public virtual bool UseSkill()
    {
        if(OnCoolDown) return false;

        Debug.Log("스킬 사용");
        remainingCoolDown = coolDown;
        return true;
    }

    public void UpdateCoolDown(float deltaTime)
    {
        if(!OnCoolDown) return;
        remainingCoolDown -= deltaTime;
    }
}
