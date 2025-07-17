using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDirection", menuName = "BehaviorTree/Actions/ChangeDirection")]
public class ChangeDirection : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        Debug.Log("울랄라");
        bool patrolling = controller.LocalBlackboard.Get<bool>("Patrolling");
        Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");
        if (!patrolling) //추적 상태중이면
        {
            if (controller.LocalBlackboard.Get<bool>("CanChangeMode")) //
            {
                controller.LocalBlackboard.Set("CanChangeMode", false);
                controller.LocalBlackboard.Set("CanAttack", false);
                controller.LocalBlackboard.Set("ModeChangeCoolDown", 0f);
                controller.LocalBlackboard.Set("Patrolling", true);
            }
        }
        controller.LocalBlackboard.Set("Direction", controller.LocalBlackboard.Get<int>("Direction") * -1); // 방향 반전

        int dir = controller.LocalBlackboard.Get<int>("Direction");
        int scale = controller.LocalBlackboard.Get<int>("Scale");

        transform.localScale = new Vector2(scale * dir, scale);
        return NodeState.Success;
    }
}
