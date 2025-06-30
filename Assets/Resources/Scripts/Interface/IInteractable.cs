using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInteractable : MonoBehaviour
{
    GameObject interactsign;
    GameObject player;
    PlayerController controller;
    bool isInteractable;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        interactsign = player.transform.GetChild(3).gameObject;
        controller = GetComponent<PlayerController>();
    }

    // 각각 범위에 있을 경우와 아닐 경우
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            interactsign.SetActive(true);
            isInteractable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            interactsign.SetActive(false);
            isInteractable= false;
        }
    }
    private void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.I))
        {
            //상호작용 메서드
            Debug.Log("상호작용");
            controller.RespawnInteract();
        }
    }
}
