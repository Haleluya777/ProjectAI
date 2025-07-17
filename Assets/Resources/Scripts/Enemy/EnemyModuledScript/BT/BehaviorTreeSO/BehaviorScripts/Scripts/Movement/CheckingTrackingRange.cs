using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingDetectionRange", menuName = "BehaviorTree/Conditions/CheckingDetectionRange")]
public class CheckingTrackingRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<float>("DistanceToPlayer") < controller.LocalBlackboard.Get<float>("DetectionRange")
        && controller.LocalBlackboard.Get<float>("DistanceToPlayer") > controller.LocalBlackboard.Get<float>("MeleeRange"))
        {
            controller.LocalBlackboard.Set("Patrolling", false);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
