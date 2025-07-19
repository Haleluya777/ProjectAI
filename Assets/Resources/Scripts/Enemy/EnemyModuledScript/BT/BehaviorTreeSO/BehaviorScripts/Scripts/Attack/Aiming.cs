using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aiming", menuName = "BehaviorTree/Actions/Aiming")]
public class Aiming : EnemyActionSO
{
    [SerializeField] private float duration;
    Vector2 aimPos;

    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.HasKey("AimingTime"))
        {
            controller.LocalBlackboard.Set("AimingTime", Time.time + duration);
            return NodeState.Running;
        }

        else
        {
            if (Time.time >= controller.LocalBlackboard.Get<float>("AimingTime"))
            {
                Debug.Log("조준 완료!");
                return NodeState.Success;
            }
            else
            {
                Debug.Log("조준 중!");
                return NodeState.Running;
            }
        }
    }
}
