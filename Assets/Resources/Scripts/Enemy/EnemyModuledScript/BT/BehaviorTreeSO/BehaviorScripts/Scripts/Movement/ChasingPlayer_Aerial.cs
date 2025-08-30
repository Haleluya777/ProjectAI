using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChasingPlayer_Aerial", menuName = "BehaviorTree/Actions/ChasingPlayer_Aerial")]
public class ChasingPlayer_Aerial : EnemyActionSO
{
    //플레이어 방향에 따라 몬스터가 바라보는 방향을 바꾸는 노드.
    //Patrolling(탐색 모드) 상태가 아닌 경우에만 발동함.
    public override NodeState Execute(EnemyAIController controller)
    {
        if (!GameManager.instance.globalBlackBoard.HasKey("PlayerCenter")) return NodeState.Failure;

        float playerX = GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position.x;
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
