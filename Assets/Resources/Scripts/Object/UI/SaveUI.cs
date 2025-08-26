using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("세이브 위치 지정");
        GameManager.instance.uIManager.saveUIPanel.GetComponent<Animator>().CrossFade("ShowSave", 0f);
        GameManager.instance.uIManager.combatUIPanel.GetComponent<Animator>().CrossFade("HideUI",0f);
    }
    public void quitinteract() 
    {
        GameManager.instance.uIManager.saveUIPanel.GetComponent<Animator>().CrossFade("HideSave", 0f);
        GameManager.instance.uIManager.combatUIPanel.GetComponent<Animator>().CrossFade("ShowUI", 0f);
    }
}
