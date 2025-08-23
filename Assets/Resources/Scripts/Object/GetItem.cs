using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour, IInteractable
{
    public bool [] gotitem = new bool[10];
    public int itemnum;
    public void Interact()
    {
        if (itemnum < 0 || itemnum >= gotitem.Length)
        {
            Debug.LogError("Item number out of range: " + itemnum);
            return;
        }
        if (!gotitem[itemnum])
        {
            gotitem[itemnum] = true;
            Debug.Log("æ∆¿Ã≈€ »πµÊ: " + itemnum);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("¿ÃπÃ »πµÊ«— æ∆¿Ã≈€: " + itemnum);
        }
    }
}
