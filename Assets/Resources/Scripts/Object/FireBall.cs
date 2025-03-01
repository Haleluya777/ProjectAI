using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : SkillObjectBase
{
    public int dmg;
    private Vector3 dir;
    private const int OBJECT_SPEED = 5;

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

    public void ObjInit(Vector3 _dir, int _dmg, string _tag)
    {
        dir = _dir;
        dmg = _dmg;
        this.gameObject.tag = _tag;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name != "Player" && other.GetComponent<IDamagable>() != null)
            other.GetComponent<IDamagable>().Damaged(dmg, this.gameObject.tag);   
    }
}
