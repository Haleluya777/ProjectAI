using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public sealed class SequenceNode : INode
{
    public string Name { get; private set; }
    public INode.NodeState LastState { get; private set; }

    List<INode> _childs;
    public SequenceNode(string name, List<INode> childs)
    {
        Name = name;
        _childs = childs;
    }

    public INode.NodeState Evaluate(IBlackBoard local, IBlackBoard global)
    {
        foreach (var child in _childs)
        {
            var state = child.Evaluate(local, global);
            if (state == INode.NodeState.Running || state == INode.NodeState.Failure)
            {
                LastState = state;
                return state;
            }
        }
        LastState = INode.NodeState.Success;
        return LastState;
    }
}
