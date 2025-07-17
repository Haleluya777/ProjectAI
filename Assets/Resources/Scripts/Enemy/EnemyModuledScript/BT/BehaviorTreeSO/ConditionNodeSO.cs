using UnityEngine;

[CreateAssetMenu(fileName = "ConditionNode", menuName = "BehaviorTree/Nodes/ConditionNode")]
public class ConditionNodeSO : BaseNodeSO
{
    public EnemyConditionSO Condition;
}