using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waiting", menuName = "BehaviorTree/Object/Platform/Actions/Waiting")]
public class Waiting : EnemyActionSO
{
    [SerializeField] private float waitingTime;

    public override NodeState Execute(EnemyAIController controller)
    {
        bool trigger = controller.LocalBlackboard.Get<bool>("Trigger");
        bool previousTrigger = controller.LocalBlackboard.Get<bool>("PreviousTrigger");

        if (previousTrigger != trigger || controller.LocalBlackboard.HasKey("WaitingTime")) //현재 트리거와 이전 트리거가 다를 때. (트리거가 바뀔 때.)
        {
            Debug.Log("대기 후 이동 합니다. 대기 시작");
            //대기 시간 활성화
            if (!controller.LocalBlackboard.HasKey("WaitingTime"))
            {
                controller.LocalBlackboard.Set("WaitingTime", Time.time + waitingTime);
                return NodeState.Running;
            }

            else
            {
                if (Time.time >= controller.LocalBlackboard.Get<float>("WaitingTime"))
                {
                    controller.LocalBlackboard.Remove("WaitingTime");
                    controller.LocalBlackboard.Set("PreviousTrigger", controller.LocalBlackboard.Get<bool>("Trigger"));
                    return NodeState.Success;
                }
                else
                {
                    return NodeState.Running;
                }
            }
        }

        else//현재 트리거와 이전 트리거가 같을 때 ()
        {
            return NodeState.Success;
        }
    }
}
