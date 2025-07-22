using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "ChasingPlayer_Aerial", menuName = "BehaviorTree/Actions/ChasingPlayer_Aerial")]
public class ChasingPlayer_Aerial : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        if (!GameManager.instance.globalBlackBoard.HasKey("PlayerTransform")) return NodeState.Failure;
        
        float playerX = GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position.x;
        Transform objTransform = controller.LocalBlackboard.Get<Transform>("Transform");

        int scale = controller.LocalBlackboard.Get<int>("Scale");
        int dir = controller.LocalBlackboard.Get<int>("Direction");

        if (!controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            dir = playerX >= controller.LocalBlackboard.Get<Transform>("Transform").position.x ? -1 : 1;
            controller.LocalBlackboard.Set("Direction", dir);
            objTransform.localScale = new Vector2(scale * dir, scale);
        }
        return NodeState.Success;
    }
}
