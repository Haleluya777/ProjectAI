/// <summary>
/// 런타임에 ActionNodeSO로부터 생성되어, 연결된 EnemyActionSO를 실행하는 노드입니다.
/// </summary>
public sealed class ActionNode : INode
{
    private readonly EnemyActionSO _action;
    
    public ActionNode(EnemyActionSO action)
    {
        _action = action;
    }

    public NodeState Evaluate(EnemyAIController controller)
    {
        if (_action == null)
        {
            return NodeState.Failure;
        }
        return _action.Execute(controller);
    }
}