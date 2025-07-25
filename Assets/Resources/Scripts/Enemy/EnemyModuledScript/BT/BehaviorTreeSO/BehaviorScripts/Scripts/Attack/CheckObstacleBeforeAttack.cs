using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckObstacleBeforeAttack",menuName ="BehaviorTree/Conditions/CheckObstacleBeforeAttack")]
public class CheckObstacleBeforeAttack : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform raycastPos = controller.LocalBlackboard.Get<Transform>("Transform");
        Vector3 raycastDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position - controller.LocalBlackboard.Get<Transform>("Transform").position).normalized;

        int layerMask = 1 << LayerMask.NameToLayer("FlatForm");
        float attRange = controller.LocalBlackboard.Get<int>("AttackType") == 0 ? controller.LocalBlackboard.Get<float>("MeleeRange") : controller.LocalBlackboard.Get<float>("RangedRange");

        if (Physics2D.Raycast(raycastPos.position, raycastDir, attRange, layerMask))
        {
            Debug.Log("장애물 있음");
            return NodeState.Failure;
        }

        return NodeState.Success;
    }
}
