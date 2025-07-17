using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject 기반의 트리(SO)를 런타임에 실행 가능한 노드 객체로 변환합니다.
/// </summary>
public static class TreeCompiler
{
    public static INode Compile(BaseNodeSO nodeSO, EnemyAIController controller)
    {
        if (nodeSO == null) return null;

        switch (nodeSO)
        {
            case ActionNodeSO action:
                return new ActionNode(action.Action);

            case ConditionNodeSO condition:
                return new ConditionNode(condition.Condition);

            case SequenceNodeSO sequence:
                var seqChildren = new List<INode>();
                foreach (var childSO in sequence.children)
                {
                    seqChildren.Add(Compile(childSO, controller));
                }
                return new SequenceNode(seqChildren);

            case SelectorNodeSO selector:
                var selChildren = new List<INode>();
                foreach (var childSO in selector.children)
                {
                    selChildren.Add(Compile(childSO, controller));
                }
                return new SelectorNode(selChildren);

            default:
                Debug.LogError($"[TreeCompiler] 알 수 없는 노드 타입입니다: {nodeSO.GetType()}");
                return null;
        }
    }
}