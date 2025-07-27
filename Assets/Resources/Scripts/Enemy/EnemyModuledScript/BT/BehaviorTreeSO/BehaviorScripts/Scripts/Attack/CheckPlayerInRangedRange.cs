using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInRangedRange", menuName = "BehaviorTree/Conditions/PlayerInRangedRange")]
public class CheckPlayerInRangedRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if(!controller.LocalBlackboard.HasKey("RangedRange") || !GameManager.instance.globalBlackBoard.HasKey("DistanceToPlayer"))
        {
            return NodeState.Failure;
        }

        float attRange = controller.LocalBlackboard.Get<float>("RangedRange");
        float escapeRange = controller.LocalBlackboard.Get<float>("EscapeRange");
        float dis = GameManager.instance.globalBlackBoard.Get<float>("DistanceToPlayer");

        if (dis <= attRange)
        {
            if (dis > escapeRange)
            {
                Debug.Log("공격 범위 안에 들어옴!");
                controller.LocalBlackboard.Set("ReadyToAttack", true);
                controller.LocalBlackboard.Set("State", 1);
                return NodeState.Success;
            }
            else
            {
                Debug.Log("도주 범위 안에 들어옴!");
                controller.LocalBlackboard.Set("ReadyToAttack", false);
                controller.LocalBlackboard.Set("State", -1);
                return NodeState.Failure;
            }
        }
        else
        {
            controller.LocalBlackboard.Set("ReadyToAttack", false);
            controller.LocalBlackboard.Set("State", 1);
            return NodeState.Failure;
        }
    }
}
