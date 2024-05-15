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
    public GameObject line;
    public GameObject particle;
    public Transform particlePos;
    public int particleCount;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        SetLine();
    }

    void FixedUpdate()
    {
        Vector3 playerPos = new Vector3(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y, 0f);
        if (aligning)
        {
            Vector3 newPos = Vector3.MoveTowards(playerPos, start.position, alignSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(newPos);
            if (Vector3.Distance(playerPos, start.position) < 0.01f)
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

            if (Vector3.Distance(playerPos, end.position) < 0.01f || player.GetComponent<PlayerController>().doubleJump == false)
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

    private void SetLine()
    {
        float xPos = ((start.position.x + end.position.x) / 2);
        float yPos = ((start.position.y + end.position.y) / 2);
        float xScale = (Mathf.Abs(Vector3.Distance(start.position, end.position)));
        float angle;
        if (Mathf.Abs(start.position.x - xPos) != 0f)
        {
            angle = Mathf.Rad2Deg * Mathf.Atan(Mathf.Abs(start.position.y - yPos) / Mathf.Abs(start.position.x - xPos));
        }
        else
        {
            angle = 90;
        }

        if (start.position.y > yPos)
        {
            angle *= -1;
        }

        line.transform.position = new Vector3(xPos, yPos, 0f);
        line.transform.localScale = new Vector3(xScale, 0.1f, 1f);
        line.transform.Rotate(0f, 0f, angle, Space.Self);

        particlePos.position = start.position;

        for (int i = 0; i < particleCount; i++)
        {
            Instantiate(particle, particlePos.position, particlePos.rotation, transform);
            particlePos.position += ((end.position - start.position) / particleCount);
        }
        Instantiate(particle, end.position, particlePos.rotation, transform);
    }
}
