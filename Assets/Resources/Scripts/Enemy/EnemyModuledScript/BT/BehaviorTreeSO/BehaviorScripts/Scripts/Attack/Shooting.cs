using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shooting", menuName = "BehaviorTree/Actions/Shooting")]
public class Shooting : EnemyActionSO
{
    [SerializeField] private int attackCount; //공격 횟수

    public override NodeState Execute(EnemyAIController controller)
    {
        Skill_Module skill = controller.LocalBlackboard.Get<Skill_Module>("Skill");
        ISkillCaster caster = controller.LocalBlackboard.Get<ISkillCaster>("SkillCaster");

        if (controller.LocalBlackboard.Get<int>("AttackTime") >= attackCount)
        {
            controller.LocalBlackboard.Set("AttackTime", 0);
            controller.LocalBlackboard.Set("Attacking", false);
            return NodeState.Success;
        }

        if (!skill.OnCoolDown)
        {
            controller.LocalBlackboard.Set("Attacking", true);
            skill.UseSkill(caster);
            controller.LocalBlackboard.Set("AttackTime", controller.LocalBlackboard.Get<int>("AttackTime") + 1);
        }
        return NodeState.Running;
    }
}
