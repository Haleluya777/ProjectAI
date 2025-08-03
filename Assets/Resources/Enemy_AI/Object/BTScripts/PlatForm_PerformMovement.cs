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
        //Vector3 destination = controller.LocalBlackboard.Get<Transform>("Destination").position;
        float moveSpeed = controller.LocalBlackboard.Get<bool>("Trigger") ? controller.LocalBlackboard.Get<float>("MoveSpeed") : controller.LocalBlackboard.Get<float>("MoveSpeed") / 2;

        if (transform.position == destination)
        {
            Debug.Log("목적지 도착");
            return NodeState.Failure;
        }
        else
        {
            Debug.Log("이동중");
            controller.LocalBlackboard.Set("CanReturnMomentum", false);
            controller.LocalBlackboard.Remove("MomentumInitTime");
            transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            return NodeState.Running;
        }
    }
}
