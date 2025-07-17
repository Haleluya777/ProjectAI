using UnityEngine;

public class EnemyAIController : MonoBehaviour, IAiManager
{
    public BaseNodeSO behaviorTreeRoot;

    // 공개 프로퍼티로 블랙보드를 노출하여 Action/Condition SO에서 쉽게 접근하도록 함
    public IBlackBoard GlobalBlackboard { get; private set; }
    public IBlackBoard LocalBlackboard { get; private set; }

    private INode _rootNode;
    
    void Awake()
    {
        _rootNode = TreeCompiler.Compile(behaviorTreeRoot, this);
    }

    public void BlackBoardInit(IBlackBoard local, IBlackBoard global)
    {
        LocalBlackboard = local;
        GlobalBlackboard = global;
    }

    void Update()
    {
        _rootNode?.Evaluate(this);
    }
}