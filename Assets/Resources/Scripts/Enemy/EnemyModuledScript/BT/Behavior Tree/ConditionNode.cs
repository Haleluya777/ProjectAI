/// <summary>
/// 런타임에 ConditionNodeSO로부터 생성되어, 연결된 EnemyConditionSO를 평가하는 노드입니다.
/// </summary>
public sealed class ConditionNode : INode
{
    private readonly EnemyConditionSO _condition;

    public ConditionNode(EnemyConditionSO condition)
    {
        _condition = condition;
    }

    public NodeState Evaluate(EnemyAIController controller)
    {
        if (_condition == null)
        {
            return NodeState.Failure;
        }
        return _condition.Evaluate(controller);
    }
}