public interface ISustainingSkill
{
    // 스킬 효과가 시작될 때 호출
    void OnSkillStart(ISkillCaster caster);
    // 스킬이 유지되는 동안 매 프레임 호출
    void OnSkillSustain(ISkillCaster caster);
    // 스킬 효과가 종료될 때 호출
    void OnSkillEnd(ISkillCaster caster);
    // 스킬의 지속 여부 (true가 되면 종료)
    bool IsFinished { get; }
}
