using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckObstacleGuarding", menuName = "BehaviorTree/Conditions/CHeckObstacleGuarding")]
public class CheckObstacleGuarding : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform raycastPos = controller.LocalBlackboard.Get<Transform>("RayCastCenterPos");
        Vector3 raycastDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position - controller.LocalBlackboard.Get<Transform>("RayCastCenterPos").position).normalized;

        int layerMask = 1 << LayerMask.NameToLayer("FlatForm");
        float guardRange = controller.LocalBlackboard.Get<float>("DistanceToPlayer");

        if (Physics2D.Raycast(raycastPos.position, raycastDir, guardRange, layerMask))
        {
            Debug.Log("장애물 있음");
            controller.LocalBlackboard.Set("Patrolling", true);
            return NodeState.Failure;
        }

        return NodeState.Success;
    }
}
