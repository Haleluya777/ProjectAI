using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerInGuardRange", menuName = "BehaviorTree/Conditions/PlayerInGuardRange")]
public class PlayerInGuardRange : EnemyConditionSO
{
    public override NodeState Evaluate(EnemyAIController controller)
    {
        if (controller.LocalBlackboard.Get<bool>("CanChangeMode") && controller.LocalBlackboard.Get<bool>("Patrolling"))
        {
            //플레이어가 경계 범위 안에 있을 경우.
            //경계 게이지를 채우는 액션 노드로 넘어가야 하기에 Success반환.
            if (controller.LocalBlackboard.Get<float>("GuardRange") >= controller.LocalBlackboard.Get<float>("DistanceToPlayer") && controller.LocalBlackboard.Get<bool>("Patrolling"))
            {
                //다만 플레이어가 Enemy의 경계 범위 안의 추적 범위 내에 존재할 경우에는 해당 시퀀스를 Failure처리 후 바로 다음 시퀀스로 넘어가야 함.
                if (controller.LocalBlackboard.Get<float>("DetectionRange") >= controller.LocalBlackboard.Get<float>("DistanceToPlayer"))
                {
                    return NodeState.Failure;
                }    
                    
                Debug.Log("경계 시작");
                controller.LocalBlackboard.Set("Guarding", true);

                return NodeState.Success;
            }
            //플레이어가 경계 범위 바깥이 있을 경우.
            //해당 시퀀스를 종료한 뒤, 다음 시퀀스로 넘어가야 하기에 Failure반환.
            else
            {
                Debug.Log("경계 종료");
                controller.LocalBlackboard.Set("Guarding", false); //경계 상태 종료
                controller.LocalBlackboard.Set("Patrolling", true); //탐색 모드 전환.
                return NodeState.Failure;
            }
        }
        else
        {
            return NodeState.Failure;
        }
        
    }
}
