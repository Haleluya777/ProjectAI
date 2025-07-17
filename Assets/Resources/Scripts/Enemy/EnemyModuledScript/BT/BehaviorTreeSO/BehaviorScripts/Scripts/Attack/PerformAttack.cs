using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PerformAttack", menuName ="BehaviorTree/Actions/PerformAttack")]
public class PerformAttack : EnemyActionSO
{
    public override NodeState Execute(EnemyAIController controller)
    {
        Animator anim = controller.LocalBlackboard.Get<Animator>("Animator");
        if (anim == null) return NodeState.Failure;

        anim.CrossFade("Enemy_Attack", 0f);
        return NodeState.Success;
    }
}
