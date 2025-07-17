using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alert", menuName = "BehaviorTree/Actions/Alert")]
public class Alert : EnemyActionSO
{
    public float waitDuration = 1f;

    public override NodeState Execute(EnemyAIController controller)
    {
        float waitTime = 0f;

        if (waitTime == 0f)
        {
            waitTime = Time.time + waitDuration;
        }

        if (Time.time < waitTime)
        {
            return NodeState.Running;
        }

        else
        {
            return NodeState.Success;
        }
    }
}
