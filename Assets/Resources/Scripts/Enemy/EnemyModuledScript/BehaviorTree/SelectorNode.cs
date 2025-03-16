using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    List<INode> _child;

    public SelectorNode(List<INode> child)
    {
        _child = child;
    }

    public INode.NodeState Evaluate()
    {
        if( _child == null )
        {
            return INode.NodeState.Failure;
        }

        foreach(INode child in _child )
        {
            switch(child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;

                case INode.NodeState.Success:
                    return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }
}
