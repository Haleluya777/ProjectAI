using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Door : MonoBehaviour, ITriggerable
{
    private SpriteRenderer render;
    private BoxCollider2D col;
    private BlackBoard local = new BlackBoard();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();

    private void OnEnable()
    {
        local.Set("Trigger", false);
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    public void Trigger(bool trigger)
    {
        render.sprite = !trigger ? sprites[0] : sprites[1];
        col.enabled = !trigger ? true : false;
    }

    public BlackBoard GetBlackBoard()
    {
        return local;
    }
}
