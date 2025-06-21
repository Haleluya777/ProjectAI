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

    public ConditionNode(string name, string operatorKey, string operandKey, Board operatorBoard, Board operandBoard, CompareMode mode)
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

        float operatorValue = OperatorBlackBoard.Get<float>(_operatorKey);
        float operandValue = OperandBlackBoard.Get<float>(_operandKey);

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
        return INode.NodeState.Failure;
    }
}
