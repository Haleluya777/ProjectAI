using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Switch : MonoBehaviour, IInteractable
{
    private List<ITriggerable> linkedObjBoard = new List<ITriggerable>();
    [SerializeField] private List<GameObject> linkedObj = new List<GameObject>();

    private BoxCollider2D col;
    private RaycastHit2D box;

    private bool trigger;

    private void OnEnable()
    {
        col = this.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        box = Physics2D.BoxCast(this.transform.position, col.bounds.size, 0, Vector2.zero, 0, 1 << 7);
        if (box.collider != null)
        {
            Debug.Log("할렐루야!");
            trigger = true;
            Interact();
        }
        else
        {
            trigger = false;
            Interact();
        }

    }

    public void Interact()
    {
        for (int i = 0; i < linkedObjBoard.Count; i++)
        {
            if (linkedObjBoard[i].GetBlackBoard().HasKey("Trigger"))
            {
                linkedObjBoard[i].GetBlackBoard().Set("Trigger", trigger);
            }

            linkedObjBoard[i].Trigger();
        }
    }
}
