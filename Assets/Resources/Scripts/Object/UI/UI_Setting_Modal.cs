using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Setting_Modal : MonoBehaviour
{
    [SerializeField] private GameObject[] modals = new GameObject[4];
    private int previousModalNum;

    private void Start()
    {
        previousModalNum = -1;
    }

    public void OpenModal(int modalNum)
    {
        if (previousModalNum == -1) previousModalNum = modalNum;
        modals[previousModalNum].SetActive(false);
        modals[modalNum].SetActive(true);
        previousModalNum = modalNum;
    }
}
