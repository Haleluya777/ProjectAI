using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy_Movement_Ground : MonoBehaviour, IMoveable, IInitializable, IRequiredAnimator
{
    private enum MovementMode {Horizontal, Vertical}
    [SerializeField] private Transform raycastPos; // 레이캐스트 시작 위치를 위한 Transform
    private Transform parentTransform;
    private Animator anim;
    private RaycastHit2D raycastHit;
    private Rigidbody2D rigid;
    private Vector2 moveDirection; // 몬스터의 현재 이동 방향
    private MovementMode mode;
    private IBlackBoard blackBoard;
    private float moveSpeed;
    private float detectionRange;
    private bool shouldMove;
    private bool patrolling;
    private bool isGround; // 지면 감지 여부
    private int dir = 1; // 몬스터의 현재 방향
    private int layerMask; // "FlatForm" 레이어 마스크
    private const int OBJ_SCALE = 2; // 오브젝트 스케일 상수
    [SerializeField] private const float groundRaycastDistance = 0.5f; // 지면 감지 레이캐스트 길이

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) // 컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        blackBoard = local;

        rigid = this.transform.parent.GetComponent<Rigidbody2D>();
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");

        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;
        detectionRange = info.Movement_Status.DetectionRange;
        patrolling = true; // 순찰 모드 활성화

        blackBoard.Set("Movement", this.GetComponent<IMoveable>());
        blackBoard.Set("DetectionRange", detectionRange);
        blackBoard.Set("Transform", parentTransform);
        blackBoard.Set("MoveSpeed", moveSpeed);
        blackBoard.Set("Patrolling", patrolling);
    }

    public void CheckingFlatForm()
    {
        raycastHit = Physics2D.Raycast(parentTransform.position, parentTransform.up * -1, groundRaycastDistance, layerMask);
        isGround = Physics2D.Raycast(raycastPos.position, raycastPos.up * -1, groundRaycastDistance, layerMask);

        if (raycastHit.collider != null)
        {
            Vector2 currentPos = parentTransform.position;
            rigid.gravityScale = 0; // 중력 비활성화
            rigid.velocity = Vector2.zero;
            shouldMove = true;

            if (parentTransform.rotation.z == 90)
            {
                parentTransform.position = new Vector2(currentPos.x, raycastHit.point.y);
            }
            else if (parentTransform.rotation.z == 0)
            {
                parentTransform.position = new Vector2(raycastHit.point.x ,currentPos.y);
            }
        }

        else
        {
            parentTransform.rotation = Quaternion.Euler(0, 0, 0);
            rigid.gravityScale = 4; // 중력 활성화
            shouldMove = false;
        }

        if (patrolling && !isGround)// && wasGround && !isGround)
        {
            dir *= -1; // 방향 반전
        }
    }

    public void UpdateDataPerFrame(IBlackBoard local) // 매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("EnemyPosition", parentTransform.position);
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void ChasingMode() //플레이어가 범위 내에 존재할 때 쫒아감.
    {
        if (!patrolling) return;
        patrolling = false;
    }

    public void PatrollingMode()
    {
        if (patrolling) return;
        patrolling = true;
    }

    public void BeIdle()
    {
        anim.CrossFade("Enemy_Idle", 0f);
    }

    public void MoveToTarget()
    {
        if (shouldMove)
        {
            anim.CrossFade("Enemy_Moving", 0f); // 애니메이션 크로스페이드
            parentTransform.Translate(Vector2.left * dir * moveSpeed * Time.deltaTime);
            UpdateDirection(GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform"));
        }
    }

    public void UpdateDirection(Transform targetPos)
    {
        if (!patrolling) // 순찰 모드가 아닐 때
        {
            // 플레이어 위치에 따라 방향 설정
            dir = parentTransform.position.x > targetPos.position.x ? 1 : -1;
        }
        transform.parent.localScale = new Vector3(OBJ_SCALE * dir, OBJ_SCALE, OBJ_SCALE);
    }
}
