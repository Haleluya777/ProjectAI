using System;
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
    public Vector3 returnspeed;
    public Vector2 initialplatformpos;
    public Vector2 curplatformpos;
    public Vector2 destinationpos;
    public bool arrive;
    bool atorigin;
    float coyoteTime = 0.2f;
    float coyoteTimeCounter;
    float reversecoyote;
    private void Start()
    {
        player = GameManager.instance.playerObj;
        rb = GetComponent<Rigidbody2D>();
        initialplatformpos = GetComponent<Transform>().position;
        destinationpos = new Vector2(initialplatformpos.x, initialplatformpos.y + 50f);
        arrive = false;
        coyoteTimeCounter = 0;
        reversecoyote = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        curplatformpos = GetComponent<Transform>().position;
        speed();
        //platformpos = player.transform.position;
        //platformpos.x += 2.5f;
        //if (transform.position.x > 10f){transform.position = new Vector3(-10f, transform.position.y, transform.position.z);}

        if (curplatformpos.y <= initialplatformpos.y)
        {
            arrive = false;
            coyoteTimeCounter = 0;
        }
        else
        {
            if (!playerOnPlatform || curplatformpos.y >= destinationpos.y) arrive = true;
        }
    }
    private void speed()
    {
        if (!arrive)//�������� �ʾҰ� �÷��̾ �÷��� ���� ���� ��
        {
            if (playerOnPlatform)
            {
                rb.velocity = movingspeed;
                coyoteTimeCounter = 0;
                if (reversecoyote < coyoteTime)
                {
                    reversecoyote += Time.fixedDeltaTime;
                    if (Input.GetButtonDown("Jump"))
                    {
                        playerOnPlatform = false;
                        platformspd = returnspeed;
                        return;
                    }
                }
                platformspd = movingspeed;
            }
            else
            {
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
        else if (arrive) //�������� ��
        {
            rb.velocity = returnspeed;
            if (playerOnPlatform)
            {
                if (coyoteTimeCounter < coyoteTime)
                {
                    coyoteTimeCounter += Time.fixedDeltaTime;
                    if (Input.GetButtonDown("Jump"))
                    {
                        playerOnPlatform = false;
                        platformspd = movingspeed;
                        return;
                    }
                }
                reversecoyote = 0;
                platformspd = returnspeed;
            }
        }
    }

    //�÷��� ���� �÷��̾ �ö�Դ���? üũ.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = true;
            Debug.Log("Player on platform");
        }
    }

    //�÷��̾ �÷��� ���� ������� üũ.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = false;
            coyoteTimeCounter = 0;
        }
    }
}
