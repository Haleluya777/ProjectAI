using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ActionNode : INode
{
    public string Name { get; private set; }
    public INode.NodeState LastState { get; private set; }

    private Func<IBlackBoard, IBlackBoard, INode.NodeState> _onUpdate;

    public ActionNode(string name, Func<IBlackBoard, IBlackBoard, INode.NodeState> onUpdate)
    {
        Name = name;
        _onUpdate = onUpdate;
    }

    public INode.NodeState Evaluate(IBlackBoard local, IBlackBoard global)
    {
        LastState = _onUpdate?.Invoke(local, global) ?? INode.NodeState.Failure;
        return LastState;
    }
}
