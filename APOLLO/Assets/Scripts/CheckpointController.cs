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
    private SpriteRenderer mySR;
    public Sprite sprite1;
    public Sprite sprite2;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
        mySR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (level.currentCheckpointID == checkpointID)
        {
            mySR.sprite = sprite2;
        }
        else
        {
            mySR.sprite = sprite1;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, checkpointID);
            level.currentCheckpointID = checkpointID;
            level.currentRoomID = roomID;
            player.respawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, 0f);
        }
    }
}
