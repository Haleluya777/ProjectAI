using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class Enemy_Movement_Ground : MonoBehaviour, IMoveable, IInitializable, IRequiredAnimator
{
    [SerializeField] private Transform raycastPos; // 레이캐스트 시작 위치를 위한 Transform
    private Transform parentTransform;
    private Animator anim;
    private RaycastHit2D raycastHit; // 변수명 변경: raycast와 헷갈리지 않게 raycastHit으로
    private Rigidbody2D rigid;
    private Vector2 moveDirection; // 몬스터의 현재 이동 방향 (로컬)
    private float moveSpeed;
    private bool shouldMove;
    private bool patrolling;
    private bool isGround; // 지면 감지 여부
    private int dir = 1; // 몬스터의 현재 방향 (좌/우 또는 상/하) - scale 및 이동 방향에 사용
    private int layerMask; // "FlatForm" 레이어 마스크
    private const int OBJ_SCALE = 2; // 오브젝트 스케일 상수
    [SerializeField] private const float groundRaycastDistance = 0.5f; // 지면 감지 레이캐스트 길이

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) // 컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        rigid = this.transform.parent.GetComponent<Rigidbody2D>();
        layerMask = 1 << LayerMask.NameToLayer("FlatForm");

        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;
        patrolling = true; // 순찰 모드 활성화

        local.Set("Movement", this.GetComponent<IMoveable>());
        local.Set("Transform", parentTransform);
        local.Set("MoveSpeed", moveSpeed);
        local.Set("Patrolling", patrolling);
    }

    public void CheckingFlatForm()
    {
        raycastHit = Physics2D.Raycast(raycastPos.position, raycastPos.up * -1, groundRaycastDistance, layerMask);
        bool wasGround = isGround;

        if (raycastHit.collider != null)
        {
            rigid.gravityScale = 0; // 중력 비활성화
            rigid.velocity = Vector2.zero;
            //parentTransform.position = raycastHit.point;
            isGround = true; // isGround 상태 업데이트
        }
        else
        {
            rigid.gravityScale = 4; // 중력 활성화
            isGround = false; // isGround 상태 업데이트
        }
        if (patrolling && wasGround && !isGround)
        {
            dir *= -1; // 방향 반전
            Debug.Log($"지면을 잃어 방향 전환: {dir}");
            Debug.Log(patrolling);
            Debug.Log(wasGround);
            Debug.Log(isGround);
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

    public void MoveToTarget()
    {
        anim.CrossFade("Enemy_Moving", 0f); // 애니메이션 크로스페이드
        parentTransform.Translate(Vector2.left * dir * moveSpeed * Time.deltaTime);

        UpdateDirection(GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform")); // 이 호출은 이제 필요 없습니다.
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
