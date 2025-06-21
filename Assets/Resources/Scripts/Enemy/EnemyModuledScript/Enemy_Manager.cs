using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.PlayerLoop;  // Count() 메서드를 사용하기 위해 필요

/// <summary>
/// Enemy의 전반적인 동작을 관리하는 메인 매니저 클래스
/// 행동 트리를 통해 Enemy의 상태와 행동을 결정
/// </summary>
public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] private EnemyStatusScriptableObject dataBase;

    [SerializeField] private Enemy_Status_Manager status;
    [SerializeField] private Enemy_Movement move;
    [SerializeField] private Enemy_Attack attack;
    [SerializeField] private EnemyAIController ai;

    [SerializeField] private IBlackBoard localBlackboard = new BlackBoard();
    [SerializeField] private EnemyUI enemyUI;

    public IDamageable istatus => status;
    public IMoveable imove => move;
    public IAttackable iattack => attack;
    public IAiManager iai => ai;

    [SerializeField] private int id;

    private BehaviorTreeRunner behaviorTree;
    private EnemyStatusInfo statusInfo;

    private void Awake()
    {
        DataInitialize(statusInfo, localBlackboard);
        iai.BlackBoardInit(localBlackboard, GameManager.instance.globalBlackBoard);
    }

    private void Update()
    {
        //move.UpdateDirection(targetObj.transform);
        UpdateDataPerFrame(localBlackboard);
    }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        info = dataBase.GetEnemyData(id);

        foreach (var init in GetComponentsInChildren<IInitializable>())
        {
            init.DataInitialize(info, local);
        }
    }

    public void UpdateDataPerFrame(IBlackBoard local)
    {
        enemyUI.HpBarUpdate(status.MaxHp, status.CurrentHp);
        //enemyUI.TextUpdate(currentState.ToString());
        foreach (var updatedData in GetComponentsInChildren<IInitializable>())
        {
            updatedData.UpdateDataPerFrame(local);
        }
    }
}
