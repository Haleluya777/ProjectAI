using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChasingPlayer_Aerial", menuName = "BehaviorTree/Actions/ChasingPlayer_Aerial")]
public class ChasingPlayer_Aerial : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        float playerX = GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position.x;

        if (!controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            Debug.Log("추적중...");
            if (playerX >= controller.LocalBlackboard.Get<Transform>("Transform").position.x)
            {
                controller.LocalBlackboard.Set("Direction", -1);
            }
            else
            {
                controller.LocalBlackboard.Set("Direction", 1);
            }
        }
        return NodeState.Success;
    }
}
