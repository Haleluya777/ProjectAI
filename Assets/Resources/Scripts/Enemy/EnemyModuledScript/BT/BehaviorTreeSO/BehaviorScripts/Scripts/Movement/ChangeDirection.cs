using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeDirection", menuName = "BehaviorTree/Actions/ChangeDirection")]
public class ChangeDirection : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.Get<bool>("isGround"))
        {
            Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");
            controller.LocalBlackboard.Set("Direction", controller.LocalBlackboard.Get<int>("Direction") * -1); // 방향 반전

            int dir = controller.LocalBlackboard.Get<int>("Direction");
            int scale = controller.LocalBlackboard.Get<int>("Scale");

            transform.localScale = new Vector2(scale * dir, scale);
        }
        return NodeState.Success;
    }
}
