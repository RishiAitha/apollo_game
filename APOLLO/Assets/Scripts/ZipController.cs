using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipController : MonoBehaviour
{
    public bool aligning;
    public bool pulling;
    private GameObject player;
    public Transform start;
    public Transform end;
    public float alignSpeed = 10f;
    public float zipSpeed = 25f;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
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
            }
        }
        if (pulling)
        {
            Vector3 newPos = Vector3.MoveTowards(playerPos, end.position, zipSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(newPos);

            if (end.position.x > start.position.x)
            {
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                player.transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            if (Vector3.Distance(playerPos, end.position) < 0.01f)
            {
                pulling = false;
                player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerController>().origGravityScale;
                start.gameObject.GetComponent<ZipPointController>().Cooldown();
                end.gameObject.GetComponent<ZipPointController>().Cooldown();
            }
        }
    }


    public void Zip()
    {
        if (!pulling && !start.gameObject.GetComponent<ZipPointController>().cooldown && !end.gameObject.GetComponent<ZipPointController>().cooldown)
        {
            aligning = true;
            player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    public void SetStart(GameObject zipPoint)
    {
        if (zipPoint.transform != start)
        {
            end = start;
            start = zipPoint.transform;
            if (pulling)
            {
                pulling = false;
                aligning = false;
                player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerController>().origGravityScale;
                start.gameObject.GetComponent<ZipPointController>().Cooldown();
                end.gameObject.GetComponent<ZipPointController>().Cooldown();
            }
        }
    }
}
