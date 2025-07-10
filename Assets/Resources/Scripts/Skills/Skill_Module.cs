using System.Collections.Generic;
using UnityEngine;

// 스킬의 "실행과 조건"을 관리하는 컨트롤러 클래스
[CreateAssetMenu(menuName = "Skill/Skill Module")]
public class Skill_Module : ScriptableObject
{
    public float coolDown;
    private float remainingCoolDown;
    public bool cancleDelay;
    public bool attackable;

    [SerializeField]
    private List<SkillBase> skills = new List<SkillBase>();

    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;

    // 스킬 사용을 시도하는 메서드
    public bool UseSkill(ISkillCaster caster)
    {
        if (OnCoolDown) return false;

        // 쿨다운이 아니라면 모든 스킬을 실행
        foreach (var skill in skills)
        {
            skill.UseSkill(caster);
        }

        // 쿨다운 시작
        remainingCoolDown = coolDown;
        return true;
    }

    // 매 프레임 쿨다운을 업데이트하는 메서드
    public void UpdateCoolDown(float deltaTime)
    {
        if (!OnCoolDown) return;
        remainingCoolDown -= deltaTime;
    }
}
