using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingFlatForm", menuName = "BehaviorTree/Conditions/CheckingFlatForm")]
public class CheckingFlatForm : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        bool isGround = controller.LocalBlackboard.Get<bool>("isGround");

        if (!isGround)
        {
            controller.LocalBlackboard.Set("isGound", false);
            return NodeState.Failure;
        }
        return NodeState.Failure;
    }
}
