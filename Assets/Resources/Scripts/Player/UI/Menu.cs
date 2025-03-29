using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    public void ActivateMenu()
    {
        Debug.Log("ÇÒ·¼·ç¾ß!");
        menuPanel.SetActive(true);
    }
}
