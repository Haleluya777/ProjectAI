using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AimAndShooting", menuName = "BehaviorTree/Actions/AimAndShooting")]
public class AimingAndShooting : EnemyActionSO
{
    [SerializeField] private float aimingTime;
    [SerializeField] private int attackCount;

    public override NodeState Execute(EnemyAIController controller)
    {
        

        return NodeState.Running;
    }
}
