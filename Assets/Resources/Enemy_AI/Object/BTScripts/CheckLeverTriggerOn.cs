using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckLeverTrigger",menuName = "BehaviorTree/Object/Platform/Conditions/CheckLeverTrigger")]
public class CheckLeverTriggerOn : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        

        return NodeState.Failure;
    }
}
