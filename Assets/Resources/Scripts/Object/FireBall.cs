using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public void ObjInit(Transform _localScale, int _dmg, string _tag)
    {
        dir = _localScale.localScale.x == -1 ? Vector3.left : Vector3.right;
        transform.localScale = _localScale.localScale;
        dmg = _dmg;
        this.gameObject.tag = _tag;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.name != "Player" && other.GetComponent<IDamagable>() != null)
            other.GetComponent<IDamagable>().Damaged(dmg, this.gameObject.tag);   
    }
}
