using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivation : MonoBehaviour
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
            if (!GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            if (GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Pause();
            }
        }
    }
}
