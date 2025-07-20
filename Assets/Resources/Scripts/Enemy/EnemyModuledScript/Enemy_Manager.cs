using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Enemy의 전반적인 동작을 관리하는 메인 매니저 클래스
/// 행동 트리를 통해 Enemy의 상태와 행동을 결정
/// </summary>
public class Enemy_Manager : MonoBehaviour
{
    [SerializeField] private EnemyStatusScriptableObject dataBase;

    [SerializeField] private IBlackBoard localBlackBoard = new BlackBoard();

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Transform trans;
    [SerializeField] private Transform rayCastTrans;

    public IDamageable istatus;
    public IMovable imove;
    public IAttackable iattack;
    public IAiManager iai;

    [SerializeField] private int id;
    private int layerMask;

    private EnemyStatusInfo statusInfo;

    private void Awake()
    {
        InterfaceInjection();
    }

    private void Start()
    {
        DataInitialize(statusInfo, localBlackBoard);
        iai.BlackBoardInit(localBlackBoard, GameManager.instance.globalBlackBoard);
    }

    private void Update()
    {
        UpdateDataPerFrame(localBlackBoard);
    }

    private void InterfaceInjection()
    {
        istatus = GetComponentInChildren<IDamageable>();
        iattack = GetComponentInChildren<IAttackable>();
        imove = GetComponentInChildren<IMovable>();
        iai = GetComponentInChildren<IAiManager>();
    }

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local)
    {
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");

        info = dataBase.GetEnemyData(id);
        anim.runtimeAnimatorController = info.Visual.Anim;

        localBlackBoard.Set("Animator", anim);
        localBlackBoard.Set("RigidBody", rigid);
        localBlackBoard.Set("Transform", trans);
        localBlackBoard.Set("LayerMask", layerMask);
        localBlackBoard.Set("RayCastTransform", rayCastTrans);

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
        foreach (var updatedData in GetComponentsInChildren<IInitializable>())
        {
            updatedData.UpdateDataPerFrame(local);
        }
    }
}
