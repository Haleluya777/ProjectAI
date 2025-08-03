using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "InitMomentum", menuName = "BehaviorTree/Object/Platform/Actions/InitMomentum")]
public class MomentumInit : EnemyActionSO
{
    [SerializeField] private float initTime;

    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.HasKey("MomentumInitTime"))
        {
            if (controller.LocalBlackboard.Get<Vector2>("CurrentMomentum") == Vector2.zero)
            {
                Debug.Log("이미 목적지에 도착하고 최대 모멘텀이 0이 되었으므로, Failure를 반환합니다.");
                return NodeState.Failure;
            }
            Debug.Log("타임 생성");
            controller.LocalBlackboard.Set("MomentumInitTime", Time.time + initTime);
            controller.LocalBlackboard.Set("CanReturnMomentum", true);
            return NodeState.Running;
        }

        else
        {
            if (Time.time >= controller.LocalBlackboard.Get<float>("MomentumInitTime"))
            {
                Debug.Log("모멘텀 전달 종료");
                controller.LocalBlackboard.Remove("MomentumInitTime");
                controller.LocalBlackboard.Set("MaxMomentum", Vector2.zero);
                controller.LocalBlackboard.Set("CanReturnMomentum", false);

                return NodeState.Success;
            }

            else
            {
                return NodeState.Running;
            }
        }
    }
}
