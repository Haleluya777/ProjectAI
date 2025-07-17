using UnityEngine;

/// <summary>
/// 모든 행동 트리 노드 SO의 기반 클래스입니다.
/// </summary>
public abstract class BaseNodeSO : ScriptableObject
{
    [Tooltip("에디터에서 식별을 위한 노드의 이름입니다.")]
    public string nodeName;
}