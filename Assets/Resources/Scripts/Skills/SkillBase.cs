using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SkillBase : ScriptableObject //모든 스킬들이 공통으로 가져야할 부분만 모아서 만든 클래스. 스크립터블 오브젝트를 상속함.
{
    //monobehavior를 상속하지 않아서 컴포넌트화를 할 수는 없다.
    public enum AttackType { Physical, Magical }

    public DmgCalculatorBase damageCalculator;
    public AttackType attackType;

    [System.NonSerialized]
    private ISkillCaster caster; //이 부분이 null인 버그 있음. 나중에 고쳐야 함. //수정 완.
    public float coolDown;
    private float remainingCoolDown;
    public bool attackable; //공격 판정이 존재하는지 여부를 체크함. True = 공격 판정을 동반한 스킬. False = 공격 판정을 동반하지 않는 스킬.
    public bool cancleDelay; //후딜 캔슬이 가능한지 여부 체크.

    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;
    public ISkillCaster Caster => caster;
    
    public virtual bool UseSkill()
    {
        if (OnCoolDown) return false;

        Debug.Log("스킬 사용");
        remainingCoolDown = coolDown;
        return true;
    }

    public void SetCaster(ISkillCaster _caster)
    {
        caster = _caster;
    }

    public void UpdateCoolDown(float deltaTime)
    {
        if (!OnCoolDown) return;
        remainingCoolDown -= deltaTime;
    }
}
