using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Guarding", menuName = "BehaviorTree/Actions/Guarding")]
public class Guard : EnemyActionSO
{
    [SerializeField] private float guardTime; //경계 시간 (유동적으로 변경 가능)

    public override NodeState Execute(EnemyAIController controller)
    {
        //경계 알고리즘.
        //경계 범위 내에 플레이어가 존재하는지 확인하는 Condition노드가 선행되어야 함.
        //이 노드에서는 GuardGage를 Time.deltatime만큼 계속 더하며, 일정 값 이상이 되면 Enemy를 추적 범위 관계 없이 무조건 추적 상태로 전환.
        //플레이어가 경계 범위 밖으로 나갈 경우 게이지 초기화.
        if (controller.LocalBlackboard.Get<bool>("Guarding"))
        {
            if (!controller.LocalBlackboard.HasKey("GuardGage")) //GuardGage키 값이 없다면.
            {
                Debug.Log("키 값 생성!");
                controller.LocalBlackboard.Set("GuardGage", Time.time + guardTime); //현재 시각 + guardTime 을 GuardGage에 셋팅한다.

                return NodeState.Running;
            }

            else //키 값이 존재. (현재 이 액션 노드에 도달한 것이 처음이 아니며, 동시에 이 노드가 Failure나 Success를 반환하지 않았음.)
            {
                if (Time.time >= controller.LocalBlackboard.Get<float>("GuardGage")) //현재 시각이 GuardGage보다 크다. (경계 상태를 guardTime만큼 유지했다.)
                {
                    Debug.Log("저기 뭐가 있다! 추적해!");
                    controller.LocalBlackboard.Remove("GuardGage"); //게이지 삭제 (더 이상 필요가 없기 때문)
                    controller.LocalBlackboard.Set("Patrolling", false); //탐색 모드 끄기.
                    return NodeState.Failure; //Failure를 반환하는 이유는 경계 시퀀스 다음이 이동 및 추적 시퀀스이기 때문. Success를 반환하면 다시 처음 시퀀스 노드부터 탐색함.
                }
                else
                {
                    Debug.Log("아직 경계 중.");
                    return NodeState.Running;
                }
            }
        }
        else
        {
            controller.LocalBlackboard.Set("Guarding", false);
            return NodeState.Failure;
        }
    }
}
