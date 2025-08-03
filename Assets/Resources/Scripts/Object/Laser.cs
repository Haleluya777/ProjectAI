using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, ITriggerable
{
    private BlackBoard local = new BlackBoard();
    IDamageable damaged;
    SpriteRenderer render;
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private int dmg;
    private RaycastHit2D rayHit;
    private BoxCollider2D col;

    private void OnEnable()
    {
        local.Set("Trigger", true);
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnDisable()
    {
        local.Clear();
    }

    public BlackBoard GetBlackBoard()
    {
        return local;
    }

    public void Trigger(bool trigger)
    {
        render.sprite = !trigger ? sprites[0] : sprites[1];
    }

    private void FixedUpdate()
    {
        rayHit = Physics2D.BoxCast(this.transform.position, col.bounds.size, 0, Vector2.zero, 0);
        if (rayHit.collider != null && rayHit.collider.tag == "Player")
        {
            Debug.Log("이얍!");
            rayHit.collider.GetComponent<IDamageable>().Damaged(dmg, "Physical");

        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (local.Get<bool>("Trigger"))
    //    {
    //        damaged = other.GetComponent<IDamageable>();
    //        if (other.gameObject.tag == "Player" && damaged != null)
    //        {
    //            Debug.Log("Get Damaged!");
    //            damaged.Damaged(dmg, "Physical");
    //        }
    //    }
    //}
}
