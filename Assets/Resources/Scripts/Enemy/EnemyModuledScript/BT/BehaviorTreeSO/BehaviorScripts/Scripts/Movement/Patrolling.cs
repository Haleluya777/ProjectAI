using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrolling", menuName = "BehaviorTree/Actions/Patrolling")]
public class Patrolling : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            Animator anim = controller.LocalBlackboard.Get<Animator>("Animator");
            Transform objTransform = controller.LocalBlackboard.Get<Transform>("Transform");

            float moveSpeed = controller.LocalBlackboard.Get<float>("MoveSpeed");
            int dir = controller.LocalBlackboard.Get<int>("Direction");

            anim.CrossFade("Enemy_Moving", 0f);
            objTransform.Translate(Vector2.left * dir * moveSpeed * Time.deltaTime);

            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
