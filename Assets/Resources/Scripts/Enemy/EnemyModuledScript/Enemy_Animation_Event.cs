using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy_Animation_Event : MonoBehaviour, IInitializable
{
    private IBlackBoard blackBoard;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) // 컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        blackBoard = local;
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {

    }

    public void AttackAnimationEnd()
    {
        blackBoard.Set("CanAttack", false);
        blackBoard.Set("MainCoolRegain", 0f);
    }
}
