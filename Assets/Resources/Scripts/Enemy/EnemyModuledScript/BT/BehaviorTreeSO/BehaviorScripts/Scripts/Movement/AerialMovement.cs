using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AerialMovement", menuName = "BehaviorTree/Actions/AerialMovement")]
public class AerialMovement : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        Debug.Log("할라할라");
        Vector3 initPos = controller.LocalBlackboard.Get<Vector3>("InitPosition");
        Vector3 destination = controller.LocalBlackboard.Get<bool>("Patrolling") ? initPos : GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position;
        Vector3 dir = Vector2.zero;

        if (controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            Debug.Log("추적 해제. 원래 자리로 돌아감");
            if (controller.LocalBlackboard.Get<Transform>("Transform").position == initPos) return NodeState.Success;
            dir = (destination - controller.LocalBlackboard.Get<Transform>("Transform").position).normalized;

        }
        else
        {
            if (GameManager.instance.globalBlackBoard.HasKey("PlayerTransform"))
            {
                Debug.Log("추적중");
                int currentState = controller.LocalBlackboard.Get<int>("State"); //플레이어를 향해 이동할지, 반대로 도망칠지 정하는 수.
                dir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position - controller.LocalBlackboard.Get<Transform>("Transform").position).normalized * currentState;
            }
        }
        controller.LocalBlackboard.Get<Transform>("Transform").Translate(dir * controller.LocalBlackboard.Get<float>("MoveSpeed") * Time.deltaTime, Space.World);
        return NodeState.Running;
    }
}
