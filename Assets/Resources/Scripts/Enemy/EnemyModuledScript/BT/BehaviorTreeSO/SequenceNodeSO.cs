using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SequenceNode", menuName = "BehaviorTree/Nodes/SequenceNode")]
public class SequenceNodeSO : BaseNodeSO
{
    [Tooltip("순서대로 실행할 자식 노드들입니다.")]
    public List<BaseNodeSO> children = new List<BaseNodeSO>();
}