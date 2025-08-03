using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private List<IBlackBoard> linkedObjBoard = new List<IBlackBoard>();
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
            linkedObjBoard.Add(linkedObj[i].GetComponent<ITriggerable>().GetBlackBoard());
        }
    }

    public void Interact()
    {
        trigger = trigger ? false : true;
        for (int i = 0; i < linkedObjBoard.Count; i++)
        {
            linkedObjBoard[i].Set("Trigger", trigger);
        }
    }
}
