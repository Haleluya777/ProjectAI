using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour, IInteractable
{
    public string Dialog { get; } = ("세이브 포인트 지정");
    PlayerController playerController;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerController.RespawnInteract();
        }
    }
}
