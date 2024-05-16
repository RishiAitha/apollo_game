using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    private GameObject player;
    public Transform start;
    public Transform end;
    public GameObject line;
    public GameObject particle;
    public Transform particlePos;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public void Teleport(Vector3 initialVelocity)
    {
        player.transform.position = end.position;
        player.GetComponent<Rigidbody2D>().velocity = initialVelocity;
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

        int particleCount = (int)Mathf.Abs(Vector3.Distance(start.position, end.position));

        for (int i = 0; i < particleCount; i++)
        {
            Instantiate(particle, particlePos.position, particlePos.rotation, transform);
            particlePos.position += ((end.position - start.position) / particleCount);
        }
        Instantiate(particle, end.position, particlePos.rotation, transform);
    }
}
