using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckShouldMove", menuName = "BehaviorTree/Conditions/CheckShouldMove")]
public class CheckShouldMove : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        bool shouldMove = controller.LocalBlackboard.Get<bool>("ShouldMove");
        if (shouldMove)
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
