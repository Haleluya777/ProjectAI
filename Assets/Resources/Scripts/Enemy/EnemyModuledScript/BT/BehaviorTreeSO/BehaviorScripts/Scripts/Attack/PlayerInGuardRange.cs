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
            Debug.Log("저기 뭔가 있는 것 같다.");
            controller.LocalBlackboard.Set("Guarding", true);
            return NodeState.Success;
        }
        else
        {
            Debug.Log("주변 이상 무.");
            controller.LocalBlackboard.Set("Guarding", false);
            return NodeState.Failure;
        }
    }
}
