using UnityEngine;

/// <summary>
/// 모든 조건 로직 ScriptableObject의 기반이 되는 추상 클래스입니다.
/// 이 클래스를 상속받아 '플레이어가 범위 내에 있는가?' 등 구체적인 조건을 구현합니다.
/// </summary>
public abstract class EnemyConditionSO : ScriptableObject
{
    public abstract NodeState Evaluate(EnemyAIController controller);
}