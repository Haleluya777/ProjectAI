using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Nodes/ActionNode")]
public class ActionNodeSO : BaseNodeSO
{
    public enum ParameterType { None, Int, Float, Bool, String, Transform }
    public enum ParameterSource { Global, Local }
    public string targetKey;  //가져올 인터페이스의 키 값.
    public string methodName;   // 행동 구분자 (실제 로직은 런타임에 대응)

    public bool hasParameter;
    public string parameterKey;
    public ParameterType parameterType;
    public ParameterSource parameterSource;
}
