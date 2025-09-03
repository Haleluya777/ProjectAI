using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlameWave : SkillObjectBase
{
    private Rigidbody2D rigid;
    private string caster;
    public int dmg;
    private Vector3 dir;
    private const int OBJECT_SPEED = 4;

    void Start()
    {
        transform.localScale = new Vector3(this.transform.localScale.x * dir.x, this.transform.localScale.y, 0);
        Destroy(this.gameObject, 1.5f);
    }

    private void Update()
    {
        Debug.Log(dir.x);
        ObjectMovment();
    }

    public override void ObjectMovment()
    {
        rigid.velocity = Vector2.right * dir.x * OBJECT_SPEED;
    }

    public override void ObjInit(Vector3 direction, int _dmg, string _tag, string _caster)
    {
        rigid = this.GetComponent<Rigidbody2D>();
        dir = direction;
        dmg = _dmg;
        this.gameObject.tag = _tag;
        caster = _caster;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != caster && other.GetComponentInChildren<IDamageable>() != null)
        {
            var damagable = other.GetComponentInChildren<IDamageable>();
            damagable.Damaged(dmg, this.gameObject.tag);
            //damagable.StatusEffectProcess(3f, "Stun");
        }
    }
}
