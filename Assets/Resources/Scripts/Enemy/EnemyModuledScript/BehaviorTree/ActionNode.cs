using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ActionNode : INode
{
    Func<INode.NodeState> _onUpdate;

    public ActionNode(Func<INode.NodeState> onUpdate)
    {
        _onUpdate = onUpdate;
    }

    public INode.NodeState Evaluate()
    {
        return _onUpdate?.Invoke() ?? INode.NodeState.Failure;
    }
}
