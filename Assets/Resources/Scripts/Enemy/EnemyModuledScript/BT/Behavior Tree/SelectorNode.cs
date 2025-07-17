using System.Collections.Generic;

/// <summary>
/// 자식 노드를 순서대로 실행하는 런타임 노드입니다.
/// 자식 중 하나라도 Success나 Running을 반환하면 즉시 중단하고 해당 상태를 반환합니다.
/// </summary>
public sealed class SelectorNode : INode
{
    private readonly List<INode> _children;

    public SelectorNode(List<INode> children)
    {
        _children = children;
    }

    public NodeState Evaluate(EnemyAIController controller)
    {
        foreach (var child in _children)
        {
            var state = child.Evaluate(controller);
            if (state != NodeState.Failure)
            {
                return state; // Success 또는 Running이면 즉시 반환
            }
        }
        return NodeState.Failure; // 모든 자식이 실패했을 때만 실패
    }
}