using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


[CreateAssetMenu(fileName = "CheckTrigger", menuName = "BehaviorTree/Object/Platform/Conditions/CheckTrigger")]
public class CheckTrigger : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<bool>("Trigger"))
        {
            return NodeState.Success;
        }
        return NodeState.Success;
    }
}
