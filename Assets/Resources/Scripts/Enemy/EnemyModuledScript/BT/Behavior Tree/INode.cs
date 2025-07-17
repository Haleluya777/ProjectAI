/// <summary>
/// 모든 런타임 노드가 반환해야 하는 상태 값입니다.
/// </summary>
public enum NodeState
{
    Running,
    Success,
    Failure,
}

/// <summary>
/// 게임 실행 중 메모리에 생성되는 모든 행동 트리 노드의 인터페이스입니다.
/// </summary>
public interface INode
{
    /// <summary>
    /// 노드의 로직을 실행하고 결과를 반환합니다.
    /// </summary>
    /// <param name="controller">트리를 실행하는 AI 컨트롤러</param>
    /// <returns>실행 결과</returns>
    NodeState Evaluate(EnemyAIController controller);
}