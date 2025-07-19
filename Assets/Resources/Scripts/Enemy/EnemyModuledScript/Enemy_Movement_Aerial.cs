using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Aerial : MonoBehaviour, IMovable, IMovable_Aerial, IInitializable, IRequiredAnimator
{
    private Transform parentTransform;
    private Transform initPos;
    private Animator anim;
    private IBlackBoard blackBoard;
    private Vector3 dir;
    private float moveSpeed;
    private bool shouldMove;
    private int angle;

    private const int OBJ_SCALE = 2;

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) //컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        blackBoard = local;

        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;

        initPos = parentTransform;

        local.Set("Movement", this.GetComponent<IMovable>());
        local.Set("Transform", parentTransform);
        local.Set("MoveSpeed", moveSpeed);
        local.Set("ShouldMove", true);
    }

    public void UpdateDataPerFrame(IBlackBoard local) //매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        MoveToTarget();
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void MoveToTarget()
    {
        Transform destination = blackBoard.Get<bool>("Patrolling") ? initPos : GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform");

        //추적, 비 추적 상태일 때의 목적지 설정.
        if (blackBoard.Get<bool>("Patrolling")) //초기 위치로 돌아감.
        {
            if (parentTransform.position == initPos.position) return;
            dir = (destination.position - parentTransform.position).normalized;
        }

        else //플레이어 추적 상태일 때.
        {
            dir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position - parentTransform.position).normalized;
        }

        if (!blackBoard.Get<bool>("ShouldMove") && !blackBoard.Get<bool>("Attacking"))
        {
            anim.CrossFade("Enemy_Moving", 0f);
            parentTransform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

            UpdateAngle();
        }
    }

    public void UpdateDirection()
    {

    }

    public void UpdateAngle()
    {

    }
}
