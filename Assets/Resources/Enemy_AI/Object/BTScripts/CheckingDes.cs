using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CheckingDes : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        Transform transform = controller.LocalBlackboard.Get<Transform>("Transform");
        Vector3 destination = controller.LocalBlackboard.Get<bool>("Trigger") ? controller.LocalBlackboard.Get<Transform>("Destination").position : controller.LocalBlackboard.Get<Vector3>("InitPos");

        if (transform.position == destination)
        {
            return NodeState.Failure;
        }

        return NodeState.Success;
    }
    
}
