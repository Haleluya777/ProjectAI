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
        float dis = GameManager.instance.globalBlackBoard.Get<float>("DistanceToPlayer");

        if (dis <= attRange)
        {
            Debug.Log("공격 범위 안에 들어옴!");
            controller.LocalBlackboard.Set("Attacking", true);
            return NodeState.Success;
        }
        else
        {
            controller.LocalBlackboard.Set("Attacking", false);
            return NodeState.Failure;
        }
    }
}
