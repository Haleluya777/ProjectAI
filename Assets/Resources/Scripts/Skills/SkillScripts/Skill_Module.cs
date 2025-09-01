using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 스킬의 "실행과 조건"을 관리하는 컨트롤러 클래스
[CreateAssetMenu(menuName = "Skill/Skill Module")]
public class Skill_Module : ScriptableObject
{
    public BlackBoard blackBoard = new BlackBoard();

    [SerializeField] private string skillName;
    [SerializeField] private string skillDetail;
    public float coolDown;

    private float remainingCoolDown;
    [SerializeField] private bool attackable; //공격 판정이 존재하는 스킬 체크
    [SerializeField] private bool cancleDelay; //기본 공격의 후딜레이를 캔슬하고 작동하는 스킬 체크
    [SerializeField] private bool havePassive; //기본 지속 효과를 가지고 있는지 여부 체크
    [SerializeField] private List<SkillBase> activeSkills = new List<SkillBase>();
    [SerializeField] private List<SkillBase> passiveSkills = new List<SkillBase>();
    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;
    public bool Attackable => attackable;
    public bool CancleDelay => cancleDelay;
    public bool HavePassive => havePassive;
    public SkillData data;

    [System.Serializable]
    public struct SkillData
    {
        public string SkillName;
        public string SkillDetail;
    }

    private void OnEnable()
    {
        data.SkillName = skillName;
        data.SkillDetail = skillDetail;

        blackBoard.Set("Condition", true);
        foreach (var skill in activeSkills)
        {
            if (skill != null) skill.Initialize(this);
        }

        foreach (var skill in passiveSkills)
        {
            if (skill != null) skill.Initialize(this);
        }
    }

    // 스킬 사용을 시도하는 메서드
    public bool UseSkill(ISkillCaster caster)
    {
        if (OnCoolDown || !blackBoard.Get<bool>("Condition"))
        {
            return false;
        }

        // 쿨다운이 아니라면 모든 스킬을 실행
        foreach (var skill in activeSkills)
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
        foreach (var skill in passiveSkills)
        {
            IPassiveSkills passiveSkill = skill as IPassiveSkills;
            if (passiveSkill != null) passiveSkill.SkillOff();
        }
        remainingCoolDown -= deltaTime;
    }

    public void ProccessPassive(ISkillCaster caster)
    {
        foreach (var skill in passiveSkills)
        {
            skill.UseSkill(caster);
        }
    }
}
