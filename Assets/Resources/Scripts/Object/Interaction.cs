using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    GameObject interactsign;
    [SerializeField] private GameObject player;
    PlayerController controller;
    bool isInteractable;
    void Start()
    {
        //player = GameObject.FindWithTag("Player");
        interactsign = player.transform.GetChild(3).gameObject;
        controller = GetComponent<PlayerController>();
    }

    // ���� ������ ���� ���� �ƴ� ���
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
            isInteractable = false;
        }
    }
    
    private void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.I))
        {
            //��ȣ�ۿ� �޼���
            Debug.Log("��ȣ�ۿ�");

            //���� ��ȣ�ۿ��� ������ ������Ʈ���� ��� ����.
            controller.RespawnInteract();
        }
    }
}
