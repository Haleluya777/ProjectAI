using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : INode
{
    public enum CompareMode { Equal, NotEqual, GreaterOrEqual, LessOrEqual, Greater, Less }
    public enum Board { local, global }

    public Board _operatorBoard; //연산자 블랙보드
    public Board _operandBoard;  //피연산자 블랙보드

    public string _operatorKey;
    public string _operandKey;    // 블랙보드 키
    public CompareMode _mode;    // 비교 방식

    public string Name { get; private set; }
    public INode.NodeState LastState { get; private set; }

    public ConditionNode(string name, string operatorKey, string operandKey, Board operatorBoard, Board operandBoard, CompareMode mode) //생성자
    {
        Name = name;
        _operatorKey = operatorKey;
        _operandKey = operandKey;
        _operatorBoard = operatorBoard;
        _operandBoard = operandBoard;
        _mode = mode;
    }

    public INode.NodeState Evaluate(IBlackBoard local, IBlackBoard global)
    {
        IBlackBoard OperatorBlackBoard = _operatorBoard == Board.local ? local : global;
        IBlackBoard OperandBlackBoard = _operandBoard == Board.local ? local : global;

        var operatorValueObj = OperatorBlackBoard.Get<object>(_operatorKey);
        var operandValueObj = OperandBlackBoard.Get<object>(_operandKey);

        if (operatorValueObj == null || operandValueObj == null)
        {
            return INode.NodeState.Failure;
        }
        if (operatorValueObj is float || operandValueObj is float)
        {
            var operatorValue = Convert.ToSingle(operatorValueObj);
            var operandValue = Convert.ToSingle(operandValueObj);

            switch (_mode)
            {
                case CompareMode.Equal:
                    return Mathf.Approximately(operatorValue, operandValue) ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.NotEqual:
                    return !Mathf.Approximately(operatorValue, operandValue) ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.GreaterOrEqual:
                    return operatorValue >= operandValue ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.LessOrEqual:
                    return operatorValue <= operandValue ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.Greater:
                    return operatorValue > operandValue ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.Less:
                    return operatorValue < operandValue ? INode.NodeState.Success : INode.NodeState.Failure;
            }
        }

        if (operatorValueObj is bool)
        {
            var operatorValue = Convert.ToBoolean(operatorValueObj);
            var operandValue = Convert.ToBoolean(operandValueObj);
            switch (_mode)
            {
                case CompareMode.Equal:
                    return (operatorValue == operandValue) ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.NotEqual:
                    return (operatorValue != operandValue) ? INode.NodeState.Success : INode.NodeState.Failure;
                default:
                    return INode.NodeState.Failure;
            }
        }

        if (operatorValueObj is IComparable comparableOperator)
        {
            int comparisonResult = comparableOperator.CompareTo(operandValueObj);
            switch (_mode)
            {
                case CompareMode.Equal:
                    return comparisonResult == 0 ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.NotEqual:
                    return comparisonResult != 0 ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.GreaterOrEqual:
                    return comparisonResult >= 0 ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.LessOrEqual:
                    return comparisonResult <= 0 ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.Greater:
                    return comparisonResult > 0 ? INode.NodeState.Success : INode.NodeState.Failure;
                case CompareMode.Less:
                    return comparisonResult < 0 ? INode.NodeState.Success : INode.NodeState.Failure;
            }
        }

        return INode.NodeState.Failure;
    }
}
