using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckDes", menuName = "BehaviorTree/Object/Platform/Conditions/CheckDes")]
public class CheckingDes : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<Vector2>("CurrentMomentum") == Vector2.zero) //현재 모멘텀이 0일 때, 즉 이동하고 있지 않을 때.
        {
            Debug.Log("이동하고 있지 않음");
            controller.LocalBlackboard.Set("Moving", false);
            return NodeState.Success;
        }
        else //모멘텀이 0이 아닐 때, 즉 이동하고 있을 때.
        {
            controller.LocalBlackboard.Set("Moving", true);
            return NodeState.Success;
        }
    }

}
