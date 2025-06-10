using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Nodes/ConditionNode")]
public class ConditionNodeSO : BaseNodeSO
{
    public enum CompareMode { Equal, NotEqual, GreaterOrEqual, LessOrEqual, Greater, Less }
    public enum Board { local, global }

    public Board operatorBoard; //연산자 블랙보드
    public Board operandBoard;  //피연산자 블랙보드드

    public string operatorKey;
    public string operandKey;    // 블랙보드 키
    public CompareMode mode;    // 비교 방식
}
