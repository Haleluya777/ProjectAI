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

    [SerializeField] private IBlackBoard localBlackBoard = new BlackBoard();
    [SerializeField] private EnemyUI enemyUI;

    [SerializeField] private Animator anim;

    public IDamageable istatus => status;
    public IMoveable imove => move;
    public IAttackable iattack => attack;
    public IAiManager iai => ai;

    [SerializeField] private int id;

    private EnemyStatusInfo statusInfo;

    private void Start()
    {
        DataInitialize(statusInfo, localBlackBoard);
        iai.BlackBoardInit(localBlackBoard, GameManager.instance.globalBlackBoard);
    }

    private void Update()
    {
        UpdateDataPerFrame(localBlackBoard);
    }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        info = dataBase.GetEnemyData(id);
        anim.runtimeAnimatorController = info.Visual.Anim;
        foreach (var init in GetComponentsInChildren<IInitializable>())
        {
            init.DataInitialize(info, local);
        }

        foreach (var init in GetComponentsInChildren<IRequiredAnimator>())
        {
            init.InjectAnimator(anim);
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
