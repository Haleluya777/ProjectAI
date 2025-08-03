using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private List<ITriggerable> linkedObjBoard = new List<ITriggerable>();
    [SerializeField] private bool trigger;
    [SerializeField] private List<GameObject> linkedObj = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Interact();
        }
    }

    private void Start()
    {
        for (int i = 0; i < linkedObj.Count; i++)
        {
            linkedObjBoard.Add(linkedObj[i].GetComponent<ITriggerable>());
        }
    }

    public void Interact()
    {
        trigger = trigger ? false : true;
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
