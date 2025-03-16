using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INode
{
    public enum NodeState { Success, Running, Failure }

    public NodeState Evaluate();
}
