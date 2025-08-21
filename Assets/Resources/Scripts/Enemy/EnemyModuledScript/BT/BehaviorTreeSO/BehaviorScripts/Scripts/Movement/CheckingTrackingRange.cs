using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingDetectionRange", menuName = "BehaviorTree/Conditions/CheckingDetectionRange")]
public class CheckingTrackingRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform raycastPos = controller.LocalBlackboard.Get<Transform>("RayCastCenterPos");
        Vector3 raycastDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position - controller.LocalBlackboard.Get<Transform>("RayCastCenterPos").position).normalized;

        int layerMask = 1 << LayerMask.NameToLayer("FlatForm");
        float detRange = controller.LocalBlackboard.Get<float>("DistanceToPlayer");

        if (controller.LocalBlackboard.Get<float>("DistanceToPlayer") <= controller.LocalBlackboard.Get<float>("DetectionRange"))
        {
            if (Physics2D.Raycast(raycastPos.position, raycastDir, detRange, layerMask))
            {

                controller.LocalBlackboard.Set("Patrolling", true);
                return NodeState.Success;
            }
            Debug.DrawRay(raycastPos.position, raycastDir * detRange, Color.red);
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
            controller.LocalBlackboard.Set("Patrolling", true);
            return NodeState.Success;
        }
    }
}
