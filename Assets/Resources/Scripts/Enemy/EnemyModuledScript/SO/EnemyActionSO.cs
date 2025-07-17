using UnityEngine;

/// <summary>
/// 모든 행동 로직 ScriptableObject의 기반이 되는 추상 클래스입니다.
/// 이 클래스를 상속받아 '이동', '공격' 등 구체적인 행동을 구현합니다.
/// </summary>
public abstract class EnemyActionSO : ScriptableObject
{
    /// <summary>
    /// 행동을 실행하고 결과를 반환합니다.
    /// </summary>
    /// <param name="controller">행동을 수행하는 주체. 블랙보드나 다른 컴포넌트에 접근할 때 사용합니다.</param>
    /// <returns>행동의 결과 (Success, Failure, Running)</returns>
    public abstract NodeState Execute(EnemyAIController controller);
}