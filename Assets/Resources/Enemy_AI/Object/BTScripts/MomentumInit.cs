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
        if (!controller.LocalBlackboard.HasKey("MomentumInitTime") && controller.LocalBlackboard.Get<int>("Count") == 0)
        {
            Debug.Log("타임 생성");
            controller.LocalBlackboard.Set("MomentumInitTime", Time.time + initTime);
            controller.LocalBlackboard.Set("CanReturnMomentum", true);
            controller.LocalBlackboard.Set("Count", controller.LocalBlackboard.Get<int>("Count") + 1);
            return NodeState.Running;
        }

        else
        {
            if (Time.time >= controller.LocalBlackboard.Get<float>("MomentumInitTime"))
            {
                controller.LocalBlackboard.Remove("MomentumInitTime");
                controller.LocalBlackboard.Set("CanReturnMomentum", false);

                return NodeState.Success;
            }

            else
            {
                Debug.Log("할랄라");
                return NodeState.Running;
            }
        }
    }
}
