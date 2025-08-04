using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    GameObject interactsign;
    [SerializeField] private GameObject player;
    bool isInteractable;
    bool isinteracting;
    void Start()
    {
        player = GameManager.instance.playerObj;
        interactsign = player.transform.GetChild(3).gameObject;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
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
            Debug.Log("세이브");
            GetComponent<IInteractable>().Interact();
            isinteracting = true;
        }
        if (!isInteractable) 
        {
            GetComponent<Spawnpoint>().quitinteract();
            isinteracting = false;
        }
        if (isinteracting && Input.GetKeyDown(KeyCode.I))
        {
            GetComponent<Spawnpoint>().quitinteract();
            isinteracting = false;
        }
    }
}
