using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInAttackDis", menuName = "BehaviorTree/Conditions/PlayerInAttackDis")]
public class CheckPlayerInAttackDis : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
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
            Debug.Log("공격 범위 밖에 있음");
            controller.LocalBlackboard.Set("Attacking", false);
            return NodeState.Failure;
        }

        //return dis <= attRange ? NodeState.Failure : NodeState.Success;
    }
}
