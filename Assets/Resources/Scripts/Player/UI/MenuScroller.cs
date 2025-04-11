using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuScroller : MonoBehaviour
{
    [SerializeField] private GameObject menuPos;

    [SerializeField] private List<GameObject> menuObjList = new List<GameObject>();

    [SerializeField] private Menu currentMenu;

    [SerializeField] private int currentIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveToPrevious();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveToNext();
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            //선택한 메뉴 활성화 시키는건데 아직 미완성.
            //currentMenu.ActivateMenu();
        }
    }

    private void MoveToPrevious() //그냥 메뉴 스크롤 하는거.
    {
        if (menuObjList.Count == 0) return;

        currentIndex = (currentIndex - 1 + menuObjList.Count) % menuObjList.Count;
        menuObjList[currentIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
        menuObjList[(currentIndex + 1) % menuObjList.Count].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        currentMenu = menuObjList[currentIndex].GetComponent<Menu>();

        UpdateMenuPosition();
    }

    private void MoveToNext() //이것도 똑같음.
    {
        if (menuObjList.Count == 0) return;

        currentIndex = (currentIndex + 1) % menuObjList.Count;
        menuObjList[currentIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(250, 250);
        menuObjList[(currentIndex - 1 + menuObjList.Count) % menuObjList.Count].GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        currentMenu = menuObjList[currentIndex].GetComponent<Menu>();

        UpdateMenuPosition();
    }

    private void UpdateMenuPosition() //메뉴 위치 바꾸는거.
    {
        for(int i = 0; i < menuObjList.Count; i++)
        {
            menuObjList[(currentIndex + i) % menuObjList.Count].transform.position = menuPos.transform.GetChild((2 + i) % menuObjList.Count).position;
        }
    }
}
