using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PullController : MonoBehaviour
{
    public bool aligning;
    public bool pulling;
    private GameObject player;
    public Transform start;
    public Transform end;
    public float alignSpeed = 10f;
    public float pullSpeed = 7.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {
        Vector3 playerPos = new Vector3(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y, 0f);
        player.GetComponent<PlayerController>().pulling = aligning || pulling;
        if (aligning)
        {
            Vector3 newPos = Vector3.MoveTowards(playerPos, start.position, alignSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(newPos);
            if (Vector3.Distance(playerPos, start.position) < 0.05f)
            {
                pulling = true;
                aligning = false;

                player.GetComponent<PlayerController>().doubleJump = true;
            }
        }
        if (pulling)
        {
            Vector3 newPos = Vector3.MoveTowards(playerPos, end.position, pullSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(newPos);

            if (end.position.x > start.position.x)
            {
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                player.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (Vector3.Distance(playerPos, end.position) < 0.05f || player.GetComponent<PlayerController>().doubleJump == false)
            {
                pulling = false;
                player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerController>().origGravityScale;
                Vector3 playerVel = player.GetComponent<Rigidbody2D>().velocity;
                if (player.GetComponent<PlayerController>().doubleJump != false)
                {
                    player.GetComponent<Rigidbody2D>().velocity = new Vector3(playerVel.x, 2f, 0f);
                }
                player.GetComponent<PlayerController>().doubleJump = false;
            }
        }
    }

    public void Pull()
    {
        if (!pulling)
        {
            aligning = true;
            player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }
}
