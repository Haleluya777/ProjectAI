using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SequenceNode : INode
{
    List<INode> _childs;
    public SequenceNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.NodeState Evaluate()
    {
        if (_childs == null || _childs.Count == 0)
        {
            return INode.NodeState.Failure;
        }

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;
                case INode.NodeState.Success:
                    continue;
                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;
            }
        }
        return INode.NodeState.Success;
    }
}
