using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Aerial : MonoBehaviour, IMovable, IMovable_Aerial, IInitializable, IRequiredAnimator
{
    private Transform parentTransform;
    private Animator anim;
    private IBlackBoard blackBoard;
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

        local.Set("Movement", this.GetComponent<IMovable>());
        local.Set("Transform", parentTransform);
        local.Set("MoveSpeed", moveSpeed);
        local.Set("ShouldMove", true);
    }

    public void UpdateDataPerFrame(IBlackBoard local) //매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("EnemyPosition", parentTransform.position);

        MoveToTarget();
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void MoveToTarget()
    {
        if (blackBoard.Get<bool>("ShouldMove") && !blackBoard.Get<bool>("Attacking"))
        {
            Debug.Log("울랄라!");
            anim.CrossFade("Enemy_Moving", 0f);
            Vector3 dir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position - parentTransform.position).normalized;
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
