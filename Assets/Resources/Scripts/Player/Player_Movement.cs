using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private Transform objTransform; //실제로 움직일 오브젝트의 Transform
    [SerializeField] private int moveSpeed; //이동 속도

    //private void Movement(Vector3 dir, Animator anim)
    //{
    //    dir = new Vector3(moveX, 0).normalized;
    //
    //    anim.SetBool("isMoving", true);
    //    transform.localScale = new Vector3(dir.x, 1, 1);
    //
    //    transform.position += dir * curMoveSpeed * Time.deltaTime;
    //    if (Input.GetKeyDown(KeyCode.X) && canDash)
    //    {
    //        Dash();
    //    }
    //}
}
