using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    private LevelManager level;

    public int roomID0;
    public int roomID1;

    public bool vertical;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!vertical)
            {
                if (level.currentRoomID == roomID0)
                {
                    level.UpdateRoom("Right");
                    level.currentRoomID = roomID1;
                }
                else
                {
                    level.UpdateRoom("Left");
                    level.currentRoomID = roomID0;
                }
            }
        }
    }
}
