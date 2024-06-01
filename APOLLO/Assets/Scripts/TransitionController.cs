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
            Debug.Log("transition entered");
            if (!vertical)
            {
                if (level.currentRoomID == roomID0)
                {
                    level.UpdateRoom("Right", gameObject);
                    level.currentRoomID = roomID1;
                }
                else
                {
                    level.UpdateRoom("Left", gameObject);
                    level.currentRoomID = roomID0;
                }
            }
            else
            {
                if (level.currentRoomID == roomID0)
                {
                    level.UpdateRoom("Up", gameObject);
                    level.currentRoomID = roomID1;
                }
                else
                {
                    level.UpdateRoom("Down", gameObject);
                    level.currentRoomID = roomID0;
                }
            }
        }
    }
}
