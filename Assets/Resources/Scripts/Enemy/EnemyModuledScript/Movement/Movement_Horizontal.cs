using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Horizontal : MonoBehaviour//, IMoveable, IInitializable, IRequiredAnimator
{
    [SerializeField] private Transform raycastPos;
    private Transform parentTransform;
    private Animator anim;
    private float moveSpeed;
    private bool shouldMove;
    private bool patrolling;
    private bool isGround;
    private int dir = 1;
    private int layerMask;

    private const int OBJ_SCALE = 2;

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, IBlackBoard local) //컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        layerMask = 1 << 6;

        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;
        patrolling = false;

        local.Set("Movement", this.GetComponent<IMoveable>());
        local.Set("Transform", parentTransform);
        local.Set("MoveSpeed", moveSpeed);
        local.Set("Patrolling", patrolling);
    }

    public void UpdateDataPerFrame(IBlackBoard local) //매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("EnemyPosition", parentTransform.position);
    }

    public void InjectAnimator(Animator _anim)
    {
        anim = _anim;
    }

    public void MoveToTarget()
    {
        isGround = Physics2D.Raycast(raycastPos.position, Vector2.down, .5f, layerMask);
        anim.CrossFade("Enemy_Moving", 0f);
        parentTransform.Translate(Vector2.left * dir * moveSpeed * Time.deltaTime);
        UpdateDirection(GameManager.instance.globalBlackBoard.Get<Transform>("PlayerTransform"));
    }

    public void UpdateDirection(Transform targetPos)
    {
        if (patrolling)
        {
            dir = isGround == true ? dir * 1 : dir * -1;
        }
        else
        {
            dir = parentTransform.position.x > targetPos.position.x ? 1 : -1;
            if (!isGround) patrolling = true;
        }
        transform.parent.localScale = new Vector3(OBJ_SCALE * dir, OBJ_SCALE, OBJ_SCALE);
    }
}
