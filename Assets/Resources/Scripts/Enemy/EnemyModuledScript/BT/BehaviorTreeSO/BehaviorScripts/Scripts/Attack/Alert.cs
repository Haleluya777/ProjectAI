using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alert", menuName = "BehaviorTree/Actions/Alert")]
public class Alert : EnemyActionSO
{
    [SerializeField] private int duration;
    public override NodeState Execute(EnemyAIController controller)
    {
        Debug.Log(controller.LocalBlackboard.HasKey("WaitTime"));
        if (!controller.LocalBlackboard.HasKey("WaitTime"))
        {
            Debug.Log("대기 시간 생성");
            controller.LocalBlackboard.Set("WaitTime", Time.time + duration);
            return NodeState.Running;
        }

        else
        {
            if (Time.time >= controller.LocalBlackboard.Get<float>("WaitTime"))
            {
                return NodeState.Success;
            }
            else
            {
                Debug.Log("경고중!");
                return NodeState.Running;
            }
        }
    }
}
