using UnityEngine;

[CreateAssetMenu(fileName = "ActionNode", menuName = "BehaviorTree/Nodes/ActionNode")]
public class ActionNodeSO : BaseNodeSO
{
    [Tooltip("이 노드가 실행할 실제 행동 로직(SO)을 여기에 연결해주세요.")]
    public EnemyActionSO Action;
}