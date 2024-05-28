using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightActivation : MonoBehaviour
{
    public LevelManager level;
    public Camera mainCamera;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        BoxCollider2D roomDimensions = level.rooms[level.currentRoomID].GetComponent<BoxCollider2D>();

        float horizontal = roomDimensions.size.x / 2;

        if (transform.position.x < mainCamera.transform.position.x + horizontal
            && transform.position.x > mainCamera.transform.position.x - horizontal
            && transform.position.y < mainCamera.transform.position.y + mainCamera.orthographicSize
            && transform.position.y > mainCamera.transform.position.y - mainCamera.orthographicSize)
        {
            if (!GetComponent<Light2D>().enabled)
            {
                GetComponent<Light2D>().enabled = true;
            }
        }
        else
        {
            if (GetComponent<Light2D>().enabled)
            {
                GetComponent<Light2D>().enabled = false;
            }
        }
    }
}
