using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireBall : SkillObjectBase
{
    //그냥 투사체.
    private string caster;
    public int dmg;
    private Vector3 dir;
    private const int OBJECT_SPEED = 10;


    private void Start() 
    {
        Destroy(gameObject, 3f);    
    }

    private void Update() 
    {
        ObjectMovment();
    }

    public override void ObjectMovment()
    {
        transform.position += dir.normalized * OBJECT_SPEED * Time.deltaTime;
    }

    public void ObjInit(Transform _localScale, int _dmg, string _tag, string _caster)
    {
        dir = _localScale.localScale.x < 0 ? Vector3.left : Vector3.right;
        transform.localScale = _localScale.localScale;
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
