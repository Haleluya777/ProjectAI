using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(menuName = "Skill/SkillBase")]
public abstract class SkillBase : ScriptableObject //모든 스킬들이 공통으로 가져야할 부분만 모아서 만든 클래스. 스크립터블 오브젝트를 상속함.
{
    //monobehavior를 상속하지 않아서 컴포넌트화를 할 수는 없다.
    public enum AttackType { Physical, Magical }

    public DmgCalculatorBase damageCalculator;
    public AttackType attackType;
    public PlayerController player; //이 부분이 null인 버그 있음. 나중에 고쳐야 함.
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
