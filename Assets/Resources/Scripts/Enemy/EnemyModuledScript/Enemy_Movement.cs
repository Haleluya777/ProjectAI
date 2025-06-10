using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Enemy의 이동과 방향 전환을 처리하는 클래스
/// </summary>
public class Enemy_Movement : MonoBehaviour, IMoveable, IInitializable
{
    private Transform parentTransform;
    private float moveSpeed;
    private bool shouldMove;
    private int dir = 1;

    public bool ShouldMove => shouldMove;
    public Transform ParentTransform => parentTransform;

    public void DataInitialize(EnemyStatusInfo info, BlackBoard local) //컴파일 될 때 최초 한번만 블랙 보드에 갱신될 정적 정보들.
    {
        parentTransform = this.transform.parent.transform;
        moveSpeed = info.Movement_Status.MoveSpeed;

        local.Set("Movement", this.GetComponent<IMoveable>());
        local.Set("Transform", parentTransform);
        local.Set("MoveSpeed", moveSpeed);
    }

    public void UpdateDataPerFrame(BlackBoard local) //매 프레임당 로컬 블랙 보드에 갱신될 정보들.
    {
        local.Set("EnemyPosition", parentTransform.position);
    }

    public void MoveToTarget(Transform target)
    {
        Debug.Log("이동중...");
        parentTransform.Translate(Vector2.left * dir * moveSpeed);
    }

    public void UpdateDirection(Transform targetPos)
    {
        dir = parentTransform.position.x > targetPos.position.x ? 1 : -1;
    }
}
