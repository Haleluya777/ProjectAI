using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class TreeCompiler
{
    public static INode Compile(BaseNodeSO nodeSO)
    {
        switch (nodeSO)
        {
            case ConditionNodeSO condition:
                return new ConditionNode(
                    name: condition.nodeName,
                    operatorKey: condition.operatorKey,
                    operandKey: condition.operandKey,
                    operatorBoard: (ConditionNode.Board)condition.operatorBoard,
                    operandBoard: (ConditionNode.Board)condition.operandBoard,
                    mode: (ConditionNode.CompareMode)condition.mode
                );

            case ActionNodeSO action:
                return new ActionNode(
                    name: action.nodeName,
                    onUpdate: (local, global) =>
                    {
                        var target = local.Get<object>(action.targetKey);
                        var method = target.GetType().GetMethod(action.methodName);

                        object[] parameters = null;
                        if (action.hasParameter)
                        {
                            IBlackBoard source = action.parameterSource == ActionNodeSO.ParameterSource.Global ? global : local;
                            switch (action.parameterType)
                            {
                                case ActionNodeSO.ParameterType.Transform:
                                    parameters = new object[] { source.Get<Transform>(action.parameterKey) };
                                    break;
                            }
                        }

                        method.Invoke(target, parameters);
                        return INode.NodeState.Success;
                    }
                );

            case SequenceNodeSO sequence:
                List<INode> seqChildren = new();
                foreach (var child in sequence.children)
                    seqChildren.Add(Compile(child));
                return new SequenceNode(sequence.nodeName, seqChildren);

            case SelectorNodeSO selector:
                List<INode> selChildren = new();
                foreach (var child in selector.children)
                    selChildren.Add(Compile(child));
                return new SelectorNode(selector.nodeName, selChildren);

            default:
                UnityEngine.Debug.LogError($"[Compiler] 알 수 없는 노드 타입: {nodeSO.name}");
                return null;
        }
    }
}
