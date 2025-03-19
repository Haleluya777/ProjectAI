using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Status status;

    private int attack;
    [SerializeField] private List<int> canUsePatternNum = new List<int>(); //사용 가능한 패턴들의 모음.
    [SerializeField] private int selectedPatternNum; //선택된 패턴의 번호

    public void StatusInit(Status status)
    {
        attack = status.Attack;
    }

    public INode NodeInitialize()
    {
        return new SequenceNode(new List<INode> {new ActionNode(PerformAction)});
    }

    private INode.NodeState CheckingAttackDistance() //공격 가능한 거리인지 확인
    {
        //근거리 공격 = 플레이어와 Enemy 유닛의 거리가 N이하일 때. (N보다 가깝거나 같을 때)
        //원거리 공격 = 플레이어와 Enemy 유닛의 거리가 M이상, K이하일 떄. ( M <= Distance <= K)
        //그런 상황일 때 Success, 아니면 Failure
        selectedPatternNum = Random.Range(0, canUsePatternNum.Count + 1);
        return INode.NodeState.Success;
    }

    private INode.NodeState DecidePattern()
    {
        //어떤 행동을 할지 고르는 노드
        //Attack 스크립트에서는 '어떤 공격 패턴'을 할지 고른다.
        //즉 사용 가능한 패턴 중, 어느 것을 사용할지 선택하는 노드.
        //그냥 랜덤 돌리면 편하다.
        //하지만 멋없쥬?

        return INode.NodeState.Success;
    }

    private INode.NodeState PerformAction() //공격 실행행
    {
        //공격 실행하고 Success 반환.
        return INode.NodeState.Success;
    }
}
