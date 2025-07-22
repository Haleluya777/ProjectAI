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
    [SerializeField] private bool arrive;
    bool atorigin;
    float coyoteTime = 0.2f;
    float coyoteTimeCounter;
    private void Start()
    {
        player = GameManager.instance.playerObj;
        //player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        initialplatformpos = GetComponent<Transform>().position;
        destinationpos = new Vector2(initialplatformpos.x, initialplatformpos.y + 50f);
        arrive = false;
        coyoteTimeCounter = 0;
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
            if (!playerOnPlatform) rb.velocity = new Vector3(0, 0, 0);
            coyoteTimeCounter = 0;
        }
        else
        {
            if (!playerOnPlatform || curplatformpos.y >= destinationpos.y) arrive = true;
        }
    }
    private void speed()
    {
        if (playerOnPlatform && !arrive)//도착하지 않았고 플레이어가 플랫폼 위에 있을 때
        {
            rb.velocity = movingspeed;
            platformspd = movingspeed;
            coyoteTimeCounter = 0;
        }
        else if (arrive) //도착했을 때
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
                platformspd = returnspeed;
            }
        }
    }

    //플랫폼 위에 플레이어가 올라왔는지 체크.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = true;
            Debug.Log("Player on platform");
        }
    }

    //플레이어가 플랫폼 위를 벗어났는지 체크.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject == player)
        {
            playerOnPlatform = false;
            coyoteTimeCounter = 0;
        }
    }
}
