using System.Collections;
using DG.Tweening;
using UnityEngine;


[CreateAssetMenu(fileName = "PlatFormMovement", menuName = "BehaviorTree/Object/Platform/Actions/PlatFormMovement")]
public class PlatForm_PerformMovement : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");
        Vector3 destination = controller.LocalBlackboard.Get<bool>("Trigger") ? controller.LocalBlackboard.Get<Transform>("Destination").position : controller.LocalBlackboard.Get<Vector3>("InitPos");

        float moveSpeed = controller.LocalBlackboard.Get<float>("MoveSpeed");
        if (transform.position == destination)
        {
            Debug.Log("목적지 도착");
            return NodeState.Failure;
        }
        else
        {
            controller.LocalBlackboard.Set("Count", 0);
            controller.LocalBlackboard.Set("CanReturnMomentum", false);
            transform.DOMove(destination, moveSpeed).SetSpeedBased().SetEase(Ease.Linear);
            return NodeState.Running;
        }
    }
}
