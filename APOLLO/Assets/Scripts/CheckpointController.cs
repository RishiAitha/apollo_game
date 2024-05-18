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

    public GameObject lightObj;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
        mySR = GetComponent<SpriteRenderer>();
        lightObj.SetActive(false);
    }

    void Update()
    {
        if (level.currentCheckpointID != checkpointID)
        {
            mySR.sprite = sprite1;
            lightObj.SetActive(false);
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
            mySR.sprite = sprite2;
            lightObj.SetActive(true);
        }
    }
}
