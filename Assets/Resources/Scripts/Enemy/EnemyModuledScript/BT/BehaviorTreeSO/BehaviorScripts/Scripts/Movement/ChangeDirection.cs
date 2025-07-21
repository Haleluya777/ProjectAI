using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDirection", menuName = "BehaviorTree/Actions/ChangeDirection")]
public class ChangeDirection : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");

        int dir = controller.LocalBlackboard.Get<int>("Direction");
        int scale = controller.LocalBlackboard.Get<int>("Scale");

        if (controller.LocalBlackboard.Get<bool>("CannotMove") && controller.LocalBlackboard.Get<bool>("ShouldMove")) //더 이상 전진할 수 없을 때 방향 전환. (공통)
        {
            dir *= -1;
            controller.LocalBlackboard.Set("Direction", dir); // 방향 반전
            transform.localScale = new Vector2(scale * dir, scale);

            if (!controller.LocalBlackboard.Get<bool>("Patrolling")) //방향을 반전할 때 추적 상태였다면.
            {
                controller.LocalBlackboard.Set("Patrolling", true); //추적 상태 해제
                controller.LocalBlackboard.Set("ModeChangeCoolDown", 0f);
                controller.LocalBlackboard.Set("CanChangeMode", false);
            }
            return NodeState.Success;
        }

        else //전진할 수 있을 때.
        {
            if (!controller.LocalBlackboard.Get<bool>("Patrolling")) //추적 상태일 때 플레이어의 위치에 따라 방향 전환.
            {
                if (!GameManager.instance.globalBlackBoard.HasKey("PlayerTransform")) return NodeState.Failure;
                dir = GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position.x < transform.position.x ? 1 : -1;
                controller.LocalBlackboard.Set("Direction", dir);
                transform.localScale = new Vector2(scale * dir, scale);
                return NodeState.Success;
            }
            return NodeState.Success;
        }
    }
}
