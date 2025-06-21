using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour, IAiManager
{
    [Header("Scriptable Tree Root")]
    public BaseNodeSO behaviorTreeAsset;
    [SerializeField] private IBlackBoard globalBlackboard ; //모든 몬스터가 공유하는 블랙보드
    [SerializeField] private IBlackBoard localBlackboard;
    private INode rootNode;

    void Awake()
    {
        rootNode = TreeCompiler.Compile(behaviorTreeAsset);
    }

    public void BlackBoardInit(IBlackBoard local, IBlackBoard global)
    {
        localBlackboard = local;
        globalBlackboard = global;
    }

    void Update()
    {
        if (localBlackboard.Get<Boolean>("CanAction") == true)
        {
            rootNode?.Evaluate(localBlackboard, globalBlackboard);
        }
    }
}
