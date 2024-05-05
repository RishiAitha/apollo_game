using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private GameObject player;
    public Transform end;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void Teleport(Vector3 initialVelocity)
    {
        player.transform.position = end.position;
        player.GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }
}
