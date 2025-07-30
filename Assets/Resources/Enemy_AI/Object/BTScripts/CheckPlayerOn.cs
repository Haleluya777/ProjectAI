using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckPlayerOn",menuName = "BehaviorTree/Object/Platform/Conditions/CheckPlayerOn")]
public class CheckPlayerOn : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");
        BoxCollider2D col = controller.LocalBlackboard.Get<BoxCollider2D>("Collider");
        int layerMask = controller.LocalBlackboard.Get<int>("LayerMask");

        if (Physics2D.BoxCast(transform.position, new Vector2(col.bounds.size.x + .1f, col.bounds.size.y + .1f), 0, Vector2.zero, 0, layerMask))
        {
            controller.LocalBlackboard.Set("Trigger", true);
            return NodeState.Success;
        }

        else
        {
            controller.LocalBlackboard.Set("Trigger", false);
            return NodeState.Success;
        }
    }
}
