using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aiming", menuName = "BehaviorTree/Actions/Aiming")]
public class Aiming : EnemyActionSO
{
    [SerializeField] private float duration;
    private Vector2 aimPos;
    private Vector3 aimDir;

    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.HasKey("AimingTime"))
        {
            controller.LocalBlackboard.Set("AimingTime", Time.time + duration);
            return NodeState.Running;
        }

        else
        {
            if (Time.time >= controller.LocalBlackboard.Get<float>("AimingTime") || controller.LocalBlackboard.Get<bool>("Attacking"))
            {
                Debug.Log("조준 완료!");
                controller.LocalBlackboard.Remove("AimingTime");
                return NodeState.Success;
            }
            else
            {
                Debug.Log("조준 중!");
                //aimDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position - controller.LocalBlackboard.Get<Transform>("Transform").position).normalized;
                //controller.LocalBlackboard.Set("AimDirection", aimDir);
                //Debug.Log(aimDir);
                return NodeState.Running;
            }
        }
    }
}
