using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    public string Name { get; private set; }
    public INode.NodeState LastState { get; private set; }

    private List<INode> _children;

    public SelectorNode(string name, List<INode> children)
    {
        Name = name;
        _children = children;
    }

    public INode.NodeState Evaluate(BlackBoard local, BlackBoard global)
    {
        foreach (var child in _children)
        {
            var state = child.Evaluate(local, global);
            if (state == INode.NodeState.Success || state == INode.NodeState.Running)
            {
                LastState = state;
                return state;
            }
        }
        LastState = INode.NodeState.Failure;
        return LastState;
    }
}
