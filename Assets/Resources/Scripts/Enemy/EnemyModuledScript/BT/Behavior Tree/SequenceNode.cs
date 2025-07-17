using System.Collections.Generic;

/// <summary>
/// 자식 노드를 순서대로 실행하는 런타임 노드입니다.
/// 자식 중 하나라도 Failure나 Running을 반환하면 즉시 중단하고 해당 상태를 반환합니다.
/// </summary>
public sealed class SequenceNode : INode
{
    private readonly List<INode> _children;

    public SequenceNode(List<INode> children)
    {
        _children = children;
    }

    public NodeState Evaluate(EnemyAIController controller)
    {
        foreach (var child in _children)
        {
            var state = child.Evaluate(controller);
            if (state != NodeState.Success)
            {
                return state; // Failure 또는 Running이면 즉시 반환
            }
        }
        return NodeState.Success; // 모든 자식이 성공했을 때만 성공
    }
}