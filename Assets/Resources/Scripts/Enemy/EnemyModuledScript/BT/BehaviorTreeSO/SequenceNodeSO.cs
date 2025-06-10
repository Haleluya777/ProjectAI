using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Nodes/SequenceNode")]
public class SequenceNodeSO : BaseNodeSO
{
    public List<BaseNodeSO> children;
}
