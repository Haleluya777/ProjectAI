using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInAttackDis", menuName = "BehaviorTree/Conditions/PlayerInAttackDis")]
public class CheckPlayerInAttackDis : EnemyConditionSO
{
    private float attRange;

    public override NodeState Evaluate(EnemyAIController controller)
    {
        attRange = controller.LocalBlackboard.Get<float>("MeleeRange");
        float dis = GameManager.instance.globalBlackBoard.Get<float>("DistanceToPlayer");

        return dis <= attRange ? NodeState.Success : NodeState.Failure;
    }
}
