using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectorNode", menuName = "BehaviorTree/Nodes/SelectorNode")]
public class SelectorNodeSO : BaseNodeSO
{
    [Tooltip("성공할 때까지 순서대로 시도할 자식 노드들입니다.")]
    public List<BaseNodeSO> children = new List<BaseNodeSO>();
}