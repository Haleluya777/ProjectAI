using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement_Aerial : MonoBehaviour, IMovable, IMovable_Aerial, IInitializable, IRequiredAnimator
{
    [SerializeField] private Transform raycastCenterPos; //Enemy 오브젝트 가운데에 위치할 레이캐스트 시작점.

    private Transform parentTransform;
    private Vector3 initPos;
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

        initPos = parentTransform.position;

        blackBoard.Set("Movement", this.GetComponent<IMovable>());
        blackBoard.Set("Transform", parentTransform);
        blackBoard.Set("InitPosition", initPos);
        blackBoard.Set("MoveSpeed", moveSpeed);
        blackBoard.Set("CanChangeMode", true);
        blackBoard.Set("ModeChangeCoolDown", 7f);
        blackBoard.Set("Direction", 1);
        blackBoard.Set("Patrolling", true);
        blackBoard.Set("ShouldMove", false);
        blackBoard.Set("RayCastCenterPos", raycastCenterPos);
    }

    public void UpdateDataPerFrame(IBlackBoard local) //매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("DistanceToPlayer", Vector2.Distance(GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position, parentTransform.position));
        //MoveToTarget();
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void MoveToTarget()
    {
        if (!GameManager.instance.globalBlackBoard.HasKey("PlayerTransform")) return;
        Vector3 destination = blackBoard.Get<bool>("Patrolling") ? initPos : GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position;

        //추적, 비 추적 상태일 때의 목적지 설정.
        if (blackBoard.Get<bool>("Patrolling")) //초기 위치로 돌아감. 비 추적 상태.
        {
            if (parentTransform.position == initPos) return;
            dir = (destination - parentTransform.position).normalized;
        }

        else //플레이어 추적 상태일 때.
        {
            if (GameManager.instance.globalBlackBoard.HasKey("PlayerTransform"))
            {
                int currentState = blackBoard.Get<int>("State"); //플레이어를 향해 이동할지, 반대로 도망칠지 정하는 수.
                dir = (GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position - parentTransform.position).normalized * currentState;
            }
        }

        if (blackBoard.Get<bool>("ShouldMove") && !blackBoard.Get<bool>("Attacking"))
        {
            anim.CrossFade("Enemy_Moving", 0f);
            parentTransform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void UpdateDirection()
    {

    }

    public void UpdateAngle()
    {

    }
}
