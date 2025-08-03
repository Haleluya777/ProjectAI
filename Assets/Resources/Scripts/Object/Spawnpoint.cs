using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("세이브 위치 지정");
        GameManager.instance.uIManager.combatUI.GetComponent<Animator>().CrossFade("HideUI",0f);
        GameManager.instance.uIManager.saveUIPanel.GetComponent<Animator>().CrossFade("ShowSave", 0f);
    }
    public void backtogame()
    {
        Debug.Log("게임으로 돌아가기");
        GameManager.instance.uIManager.saveUIPanel.GetComponent<Animator>().CrossFade("HideSave", 0f);
        GameManager.instance.uIManager.combatUI.GetComponent<Animator>().CrossFade("ShowUI", 0f);
    }
}
