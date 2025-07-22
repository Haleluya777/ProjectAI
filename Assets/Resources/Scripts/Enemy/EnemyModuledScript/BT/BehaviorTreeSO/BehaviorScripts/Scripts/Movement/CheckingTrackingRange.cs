using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingDetectionRange", menuName = "BehaviorTree/Conditions/CheckingDetectionRange")]
public class CheckingTrackingRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<float>("DistanceToPlayer") <= controller.LocalBlackboard.Get<float>("DetectionRange"))
        {
            if (controller.LocalBlackboard.Get<bool>("CanChangeMode"))
            {
                Debug.Log("추적모드 전환");
                controller.LocalBlackboard.Set("Patrolling", false);
                return NodeState.Success;
            }
            else
            {
                controller.LocalBlackboard.Set("Patrolling", true);
                return NodeState.Success;
            }
        }
        else
        {
            Debug.Log("탐색 모드 전환");
            return NodeState.Success;
        }
    }
}
