using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCircle : SkillObjectBase
{
    private Rigidbody2D rigid;
    private string caster;
    public int dmg;
    private Vector3 dir;

    [Tooltip("목적지")]
    private Vector3 des;
    private const int OBJECT_SPEED = 20;
    void Start()
    {
        //오브젝트 삭제 코드. 추후 오브젝트 풀로 돌려보내는 코드로 변경 예정.
        Destroy(this.gameObject, 2f);
    }

    private void FixedUpdate()
    {
        //Debug.Log(((Vector2)des - rigid.position).magnitude);
        ObjectMovment();
    }

    public override void ObjectMovment()
    {

        if (((Vector2)des - rigid.position).magnitude <= .5f)
        {
            rigid.velocity = Vector2.zero;
            return;
        }
        rigid.velocity = Vector2.right * dir.x * OBJECT_SPEED;
    }

    public override void ObjInit(Vector3 direction, int _dmg, string _tag, string _caster)
    {
        rigid = this.GetComponent<Rigidbody2D>();
        dir = direction;

        des = new Vector3(rigid.position.x + (10 * dir.x), rigid.position.y, 0);
        dmg = _dmg;
        this.gameObject.tag = _tag;
        caster = _caster;
    }
}
