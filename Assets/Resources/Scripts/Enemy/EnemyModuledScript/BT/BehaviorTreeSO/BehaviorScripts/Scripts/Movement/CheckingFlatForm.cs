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
            Debug.Log("방향을 전환합니다!");
            return NodeState.Success;
        }
        Debug.Log("실패!");
        return NodeState.Failure;
    }
}
