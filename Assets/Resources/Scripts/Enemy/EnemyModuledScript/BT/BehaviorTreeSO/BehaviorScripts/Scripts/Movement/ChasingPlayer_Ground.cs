using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChasingPlayer", menuName = "BehaviorTree/Actions/ChasingPlayer")]
public class ChasingPlayer_Ground : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            Debug.Log("추적중..");
            Transform objTransform = controller.LocalBlackboard.Get<Transform>("Transform");
            Transform playerTransform = GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform");

            int scale = controller.LocalBlackboard.Get<int>("Scale");
            int dir = controller.LocalBlackboard.Get<int>("Direction");

            if (controller.LocalBlackboard.Get<bool>("CannotMove"))//추적 중인데 전진 불가능한 경우.
            {
                Debug.Log("추적 해제");
                controller.LocalBlackboard.Set("CanChangeMode", false);
                controller.LocalBlackboard.Set("ModeChangeCoolDown", 0f);
                return NodeState.Failure;
            }

            else
            {
                if (controller.LocalBlackboard.Get<int>("MovementMode") == 0) //Mode = Horizontal일 때. (수평이동)
                {
                    dir = objTransform.position.x > playerTransform.position.x ? 1 : -1;
                    controller.LocalBlackboard.Set("Direction", dir);
                }

                else //Mode = Veritcal일 때. (수직이동)
                {
                    dir = objTransform.position.y > playerTransform.position.y ? 1 : -1;
                    controller.LocalBlackboard.Set("Direction", dir);
                }
                objTransform.localScale = new Vector2(scale * dir, scale);
            }
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
