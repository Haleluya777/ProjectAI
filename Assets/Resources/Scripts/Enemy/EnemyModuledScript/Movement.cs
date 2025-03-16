using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMovable
{
    [SerializeField] private Status status;

    public void Move(int moveSpeed, GameObject target) //실제로 이동하는 메서드
    {

    }

    public INode NodeInitialize()
    {
        return new SequenceNode(new List<INode> { new ActionNode(PerformMoving) });
    }

    private INode.NodeState CheckingDistance()
    {
        return INode.NodeState.Success;
    }

    private INode.NodeState PerformMoving()
    {
        return INode.NodeState.Running;
    }
}
