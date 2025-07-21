using System.Collections.Generic;

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
                return state;
            }
        }
        return NodeState.Failure;
    }
}