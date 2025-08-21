using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingObstacleChasing", menuName = "BehaviorTree/Conditions/CheckingObstacleChasing")]
public class CheckingObstacleChasing : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform trans = controller.LocalBlackboard.Get<Transform>("Transform");
        Transform raycastPos = controller.LocalBlackboard.Get<Transform>("Transform");
        Vector3 raycastDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position - raycastPos.position).normalized;

        int layerMask = 1 << LayerMask.NameToLayer("FlatForm");
        float detRange = controller.LocalBlackboard.Get<float>("DetectionRange");

        RaycastHit2D ray = Physics2D.Raycast(raycastPos.position, raycastDir, detRange, layerMask);

        if (ray.collider != null && !controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            Debug.Log("추적 중인데, 장애물 있음");
            Debug.Log(ray.collider.name);

            controller.LocalBlackboard.Set("Direction", -1 * controller.LocalBlackboard.Get<int>("Direction"));
            trans.localScale = new Vector2(controller.LocalBlackboard.Get<int>("Scale") * controller.LocalBlackboard.Get<int>("Direction"), controller.LocalBlackboard.Get<int>("Scale"));

            controller.LocalBlackboard.Set("Patrolling", true); //추적 상태 해제
            controller.LocalBlackboard.Set("Guarding", false);
            controller.LocalBlackboard.Remove("GuardGage");
            controller.LocalBlackboard.Set("ModeChangeCoolDown", 0f);
            controller.LocalBlackboard.Set("CanChangeMode", false);

            return NodeState.Success;
        }

        return NodeState.Success;
    }
}
