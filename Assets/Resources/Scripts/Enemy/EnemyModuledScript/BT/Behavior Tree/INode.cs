using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum NodeState { Success, Running, Failure }

    string Name { get; }
    NodeState LastState { get; }

    public NodeState Evaluate(IBlackBoard local, IBlackBoard global);
}
