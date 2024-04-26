using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{
    public int checkpointID;
    public int roomID;
    private LevelManager level;
    private PlayerController player;
    private bool active;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (level.currentCheckpointID == checkpointID)
        {
            active = true;
        }

        if (active)
        {
            // change sprite here
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, checkpointID);
            level.currentCheckpointID = checkpointID;
            level.currentRoomID = roomID;
            player.respawnPosition = transform.position;
        }
    }
}
