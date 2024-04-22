using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    private LevelManager level;

    public int roomID0;
    public int roomID1;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (level.currentRoomID == roomID0)
            {
                level.currentRoomID = roomID1;
            }
            else
            {
                level.currentRoomID = roomID0;
            }
            level.UpdateRoom();
        }
    }
}
