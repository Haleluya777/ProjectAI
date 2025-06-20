using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [Header("Scriptable Tree Root")]
    public BaseNodeSO behaviorTreeAsset;

    [Header("Blackboards")]
    [SerializeField] private BlackBoard globalBlackboard ; //모든 몬스터가 공유하는 블랙보드
    [SerializeField] private BlackBoard localBlackboard;
    private INode rootNode;

    void Awake()
    {
        rootNode = TreeCompiler.Compile(behaviorTreeAsset);
    }

    void Update()
    {
        if (localBlackboard.Get<Boolean>("CanAction") == true)
        {
            rootNode?.Evaluate(localBlackboard, globalBlackboard);
        }
    }
}
