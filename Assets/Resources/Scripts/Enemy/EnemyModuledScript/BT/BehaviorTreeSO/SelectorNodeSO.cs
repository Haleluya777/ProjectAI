using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Nodes/SelectorNode")]
public class SelectorNodeSO : BaseNodeSO
{
    public List<BaseNodeSO> children;
}
