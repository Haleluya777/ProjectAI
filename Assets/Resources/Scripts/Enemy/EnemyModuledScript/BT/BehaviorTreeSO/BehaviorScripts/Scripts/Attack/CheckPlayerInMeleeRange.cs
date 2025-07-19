using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInMeleeRange", menuName = "BehaviorTree/Conditions/PlayerInMeleeRange")]
public class CheckPlayerInMeleeRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if(!controller.LocalBlackboard.HasKey("MeleeRange") || !GameManager.instance.globalBlackBoard.HasKey("DistanceToPlayer"))
        {
            return NodeState.Failure;
        }

        float attRange = controller.LocalBlackboard.Get<float>("MeleeRange");
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
