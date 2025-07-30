using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Waiting", menuName = "BehaviorTree/Object/Platform/Actions/Waiting")]
public class Waiting : EnemyActionSO
{
    [SerializeField] private float waitingTime;

    public override NodeState Execute(EnemyAIController controller)
    {
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
                return NodeState.Success;
            }
            else
            {
                return NodeState.Running;
            }
        }
    }
}
