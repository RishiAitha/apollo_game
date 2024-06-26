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
    public GameObject line;
    public GameObject particle;
    public Transform particlePos;
    public float stuckTime;
    public float stuckCounter;
    private Vector3 previousPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        SetParticles();
    }

    void FixedUpdate()
    {
        SetLine();
        Vector3 playerPos = new Vector3(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y, 0f);
        if (aligning)
        {
            Vector3 newPos = Vector3.MoveTowards(playerPos, start.position, alignSpeed * Time.deltaTime);
            player.GetComponent<Rigidbody2D>().MovePosition(newPos);

            if (Mathf.Abs(Vector3.Distance(player.transform.position, previousPosition)) < 0.1f)
            {
                stuckCounter += Time.deltaTime;
            }
            else
            {
                stuckCounter = 0f;
            }

            previousPosition = player.transform.position;

            if (stuckCounter >= stuckTime)
            {
                pulling = false;
                aligning = false;
                player.GetComponent<Rigidbody2D>().gravityScale = player.GetComponent<PlayerController>().origGravityScale;
            }

            if (Vector3.Distance(playerPos, start.position) < 0.01f)
            {
                pulling = true;
                aligning = false;
                stuckCounter = 0f;
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

            if (Mathf.Abs(Vector3.Distance(player.transform.position, previousPosition)) < 0.01f)
            {
                stuckCounter += Time.deltaTime;
            }
            else
            {
                stuckCounter = 0f;
            }

            previousPosition = player.transform.position;

            if (Vector3.Distance(playerPos, end.position) < 0.001f || stuckCounter >= stuckTime)
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
