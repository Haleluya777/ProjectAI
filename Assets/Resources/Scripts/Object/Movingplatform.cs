using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class Movingplatform : MonoBehaviour
{
    public bool playerOnPlatform = false;
    public GameObject player;
    Rigidbody2D rb;
    public Vector3 platformspd;
    public Vector3 movingspeed;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerOnPlatform)
        {
            platformspd = movingspeed;
            //platformpos = player.transform.position;
            //platformpos.x += 2.5f;
            //if (transform.position.x > 10f){transform.position = new Vector3(-10f, transform.position.y, transform.position.z);}
        }
        else platformspd = new Vector3(0, 0, 0);
        rb.velocity = platformspd;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = true;
            Debug.Log("Player on platform");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = false;
        }
    }
}
