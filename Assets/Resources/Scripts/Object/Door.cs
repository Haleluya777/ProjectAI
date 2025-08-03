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

    public void Trigger()
    {
        render.sprite = !local.Get<bool>("Trigger") ? sprites[0] : sprites[1];
        col.enabled = !local.Get<bool>("Trigger") ? true : false;
    }

    public BlackBoard GetBlackBoard()
    {
        return local;
    }
}
