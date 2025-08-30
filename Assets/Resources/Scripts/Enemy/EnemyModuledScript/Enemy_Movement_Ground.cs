using System;
using UnityEngine;

public class Enemy_Movement_Ground : MonoBehaviour, IMovable, IInitializable, IRequiredAnimator
{
    private enum MovementMode { Horizontal, Vertical }
    [SerializeField] private Transform raycastPos; // 레이캐스트 시작 위치를 위한 Transform
    [SerializeField] private Transform raycastCenterPos; //Enemy 오브젝트 가운데에 위치할 레이캐스트 시작점.
    private Transform parentTransform;
    private Animator anim;
    private RaycastHit2D raycastHit;
    private Rigidbody2D rigid;
    private Vector2 moveDirection; // 몬스터의 현재 이동 방향
    private IBlackBoard blackBoard;
    private float moveSpeed;
    private bool shouldMove;
    private bool patrolling;
    private int dir = 1; // 몬스터의 현재 방향
    private int layerMask; // "FlatForm" 레이어 마스크
    private const int OBJ_SCALE = 2; // 오브젝트 스케일 상수
    [SerializeField] private const float groundRaycastDistance = 0.5f; // 지면 감지 레이캐스트 길이
    [SerializeField] private float frontRayCastDistance = 1f; //전방 장애물 감지 레이 캐스트 길이
    [SerializeField] private float canDetectionCool;

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) // 컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        blackBoard = local;

        rigid = this.transform.parent.GetComponent<Rigidbody2D>();
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");

        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;
        patrolling = true; // 순찰 모드 활성화

        blackBoard.Set("Movement", this.GetComponent<IMovable>());
        blackBoard.Set("Transform", parentTransform);
        blackBoard.Set("MoveSpeed", moveSpeed);
        blackBoard.Set("CanChangeMode", true);
        blackBoard.Set("ModeChangeCoolDown", 7f);
        blackBoard.Set("Direction", 1);
        blackBoard.Set("Patrolling", true);
        blackBoard.Set("ShouldMove", false);
        blackBoard.Set("MovementMode", MovementMode.Horizontal);
        blackBoard.Set("RayCastCenterPos", raycastCenterPos);
    }

    public void CheckingFlatForm()
    {
        raycastHit = Physics2D.Raycast(parentTransform.position, parentTransform.up * -1, groundRaycastDistance, layerMask);
        Vector2 raycastDir = (Vector2.right * blackBoard.Get<int>("Direction"));
        Debug.DrawRay(new Vector2(raycastPos.position.x, raycastPos.position.y + 0.1f), raycastDir * frontRayCastDistance, Color.red);
        if (!Physics2D.Raycast(raycastPos.position, raycastPos.up * -1, groundRaycastDistance, layerMask) || Physics2D.Raycast(new Vector2(raycastPos.position.x, raycastPos.position.y + 0.1f), raycastDir, frontRayCastDistance, layerMask))
        {
            blackBoard.Set("CannotMove", true);
        }
        else
        {
            blackBoard.Set("CannotMove", false);
        }

        if (raycastHit.collider != null)
        {
            Vector2 currentPos = parentTransform.position;
            rigid.gravityScale = 0; // 중력 비활성화
            rigid.velocity = Vector2.zero;
            blackBoard.Set<bool>("ShouldMove", true);

            if (parentTransform.rotation.z == 90)
            {
                parentTransform.position = new Vector2(currentPos.x, raycastHit.point.y);
                blackBoard.Set("MovementMode", MovementMode.Vertical);
            }
            else if (parentTransform.rotation.z == 0)
            {
                parentTransform.position = new Vector2(raycastHit.point.x, currentPos.y);
                blackBoard.Set("MovementMode", MovementMode.Horizontal);
            }
        }

        else //땅, 혹은 벽을 딛고 있지 않을 때.
        {
            parentTransform.rotation = Quaternion.Euler(0, 0, 0);
            rigid.gravityScale = 4; // 중력 활성화
            blackBoard.Set<bool>("ShouldMove", false);
        }
    }

    public void UpdateDataPerFrame(IBlackBoard local) // 매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("EnemyPosition", parentTransform.position);
        local.Set("ModeChangeCoolDown", local.Get<float>("ModeChangeCoolDown") + Time.deltaTime);

        if (!GameManager.instance.globalBlackBoard.HasKey("PlayerTransform")) return;
        local.Set("DistanceToPlayer", Vector2.Distance(GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform").position, parentTransform.position));

        if (local.Get<float>("ModeChangeCoolDown") >= 1f)
        {
            local.Set("CanChangeMode", true);
        }
        CheckingFlatForm();
    }

    public void MoveToTarget()
    {

    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void PatrollingMode() //추적 모드 해제 및 일정 시간 동안 추적 불가.
    {
        if (patrolling) return;
        patrolling = true;
    }

    public void BeIdle()
    {
        anim.CrossFade("Enemy_Idle", 0f);
    }

    public void UpdateDirection()
    {
        dir = blackBoard.Get<int>("Direction");
        transform.parent.localScale = new Vector3(OBJ_SCALE * -dir, OBJ_SCALE, OBJ_SCALE);
    }
}
