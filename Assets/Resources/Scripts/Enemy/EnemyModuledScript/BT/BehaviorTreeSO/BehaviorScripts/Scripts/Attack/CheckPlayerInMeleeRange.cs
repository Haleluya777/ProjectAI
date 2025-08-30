using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInAttackRange", menuName = "BehaviorTree/Conditions/PlayerInAttackRange")]
public class CheckPlayerInMeleeRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.HasKey("MeleeRange") || !controller.LocalBlackboard.HasKey("DistanceToPlayer"))
        {
            return NodeState.Failure;
        }

        if (controller.LocalBlackboard.Get<float>("DistanceToPlayer") <= controller.LocalBlackboard.Get<float>("MeleeRange"))
        {
            Debug.Log("공격 범위 안에 들어옴!");
            controller.LocalBlackboard.Set("ReadyToAttack", true);
            return NodeState.Success;
        }
        else
        {
            Debug.Log("공격 범위 밖임");
            controller.LocalBlackboard.Set("ReadyToAttack", false);
            return NodeState.Failure;
        }
    }
}
