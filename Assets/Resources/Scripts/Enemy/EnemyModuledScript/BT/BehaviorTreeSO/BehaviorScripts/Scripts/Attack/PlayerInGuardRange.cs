using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerInGuardRange", menuName = "BehaviorTree/Conditions/PlayerInGuardRange")]
public class PlayerInGuardRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<float>("GuardRange") <= GameManager.instance.globalBlackBoard.Get<float>("DistanceToPlayer"))
        {
            controller.LocalBlackboard.Set("Guarding", true);
            return NodeState.Success;
        }
        else
        {
            controller.LocalBlackboard.Set("Guarding", false);
            controller.LocalBlackboard.Set("GuardGage", 0f);
            return NodeState.Failure;
        }
    }
}
