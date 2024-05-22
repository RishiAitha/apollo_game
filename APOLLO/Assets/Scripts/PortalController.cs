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
        if (line != null)
        {
            SetParticles();
        }
        else
        {
            Instantiate(particle, start.position, particlePos.rotation, start);
            Instantiate(particle, end.position, particlePos.rotation, end);
        }
    }

    void Update()
    {
        if (line != null)
        {
            SetLine();
        }
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

        if (!(start.position.x > xPos && start.position.y > yPos) && (start.position.x > xPos || start.position.y > yPos))
        {
            angle *= -1;
        }

        if (start.position.x > xPos)
        {
            xScale *= -1;
        }

        line.transform.position = new Vector3(xPos, yPos, 0f);
        line.transform.localScale = new Vector3(xScale, 0.1f, 1f);
        line.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private void SetParticles()
    {
        particlePos.localPosition = new Vector3(-line.transform.localScale.x / 2, 0f, 0f);

        int particleCount = (int)(1.5 * Mathf.Abs(Vector3.Distance(start.position, end.position)));

        for (int i = 0; i <= particleCount; i++)
        {
            GameObject newParticle = Instantiate(particle, line.transform, false);
            newParticle.transform.localPosition = particlePos.localPosition;
            newParticle.transform.localRotation = particlePos.localRotation;
            particlePos.localPosition = new Vector3(particlePos.localPosition.x + (line.transform.localScale.x / particleCount), 0f, 0f);
        }
    }
}
