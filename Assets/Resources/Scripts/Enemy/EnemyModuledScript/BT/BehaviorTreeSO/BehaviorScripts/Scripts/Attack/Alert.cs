using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alert", menuName = "BehaviorTree/Actions/Alert")]
public class Alert : EnemyActionSO
{
    [SerializeField] private int duration;
    [SerializeField] private bool canAiming;

    private Vector2 aimPos;
    private Vector3 aimDir;

    public override NodeState Execute(EnemyAIController controller)
    {
        if (!controller.LocalBlackboard.HasKey("WaitTime"))
        {
            Debug.Log("대기 시간 생성");
            controller.LocalBlackboard.Set("WaitTime", Time.time + duration);
            return NodeState.Running;
        }

        else
        {
            if (controller.LocalBlackboard.Get<bool>("Attacking"))
            {
                return NodeState.Success;
            }

            if (Time.time >= controller.LocalBlackboard.Get<float>("WaitTime"))
            {
                controller.LocalBlackboard.Remove("WaitTime");
                return NodeState.Success;
            }
            else
            {
                Debug.Log("경고중!");
                if (canAiming)
                {
                    aimDir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerCenter").position - controller.LocalBlackboard.Get<Transform>("ShootingPos").position).normalized;
                    controller.LocalBlackboard.Set("AimDirection", aimDir);
                    //Debug.Log(aimDir);
                }
                return NodeState.Running;
            }
        }
    }
}
