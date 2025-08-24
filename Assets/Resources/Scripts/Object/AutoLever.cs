using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLever : MonoBehaviour, IInteractable
{
    private List<ITriggerable> linkedObjBoard = new List<ITriggerable>(); //연결된 오브젝트의 블랙 보드.
    [SerializeField] private List<GameObject> linkedObj = new List<GameObject>(); //연결된 오브젝트

    private BoxCollider2D col;
    private RaycastHit2D box;
    private bool trigger;

    [SerializeField] private float timer;

    private void OnEnable()
    {
        col = this.GetComponent<BoxCollider2D>();
        for (int i = 0; i < linkedObj.Count; i++)
        {
            linkedObjBoard.Add(linkedObj[i].GetComponent<ITriggerable>());
        }
    }

    void FixedUpdate()
    {

    }

    public void Interact()
    {
        if (trigger) return;
        for (int i = 0; i < linkedObjBoard.Count; i++)
        {
            if (linkedObjBoard[i].GetBlackBoard().HasKey("Trigger"))
            {
                linkedObjBoard[i].GetBlackBoard().Set("Trigger", true);
            }

            linkedObjBoard[i].Trigger();
        }

        Invoke("TriggerOff", timer);
    }

    private void TriggerOff()
    {
        for (int i = 0; i < linkedObjBoard.Count; i++)
        {
            if (linkedObjBoard[i].GetBlackBoard().HasKey("Trigger"))
            {
                linkedObjBoard[i].GetBlackBoard().Set("Trigger", false);
            }

            linkedObjBoard[i].Trigger();
        }
        return;
    }
}
