using System.Collections;
using System.Collections.Generic;
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
    public float pullSpeed = 5f;

    void Start ()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        player.GetComponent<PlayerController>().pulling = pulling;
        if (aligning)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, start.position, alignSpeed * Time.deltaTime);
            if (Vector3.Distance(player.transform.position, start.position) < 0.05f)
            {
                pulling = true;
                aligning = false;
            }
        }
        if (pulling)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, end.transform.position, pullSpeed * Time.deltaTime);
            if (Vector3.Distance(player.transform.position, end.position) < 0.05f)
            {
                pulling = false;
                player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerController>().origGravityScale;
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
