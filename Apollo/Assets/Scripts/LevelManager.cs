using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentRoomID;
    public GameObject[] rooms;

    private Camera camera;

    private bool movingCamera;
    private bool foundPosition;

    public float cameraSpeed;

    private PlayerController player;

    private float vertical;
    private float horizontal;
    private float cameraSize;
    private Vector3 cameraPos;

    private string transitionDirection;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        camera = FindObjectOfType<Camera>();

        transitionDirection = "Right";

        movingCamera = false;
        foundPosition = false;

        currentRoomID = 0;
    }

    void Update()
    {
        if (movingCamera && !foundPosition)
        {
            player.changingRooms = true;
            player.gameRunning = false;
            BoxCollider2D roomDimensions = rooms[currentRoomID].GetComponent<BoxCollider2D>();

            if (roomDimensions != null)
            {
                vertical = roomDimensions.size.y;
                horizontal = roomDimensions.size.x * camera.pixelHeight / camera.pixelWidth;

                cameraSize = Mathf.Max(vertical, horizontal) * 0.5f;

                cameraPos = new Vector3(roomDimensions.gameObject.transform.position.x, roomDimensions.gameObject.transform.position.y, -10f);
                foundPosition = true;
            }
        }

        if (movingCamera && foundPosition)
        {
            camera.gameObject.transform.position = Vector3.Lerp(camera.gameObject.transform.position, cameraPos, cameraSpeed * Time.deltaTime);
            
            camera.orthographicSize = cameraSize;

            if (transitionDirection == "Right")
            {
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(1f, player.gameObject.GetComponent<Rigidbody2D>().velocity.y, 0f);
            }
            else if (transitionDirection == "Left")
            {
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-1f, player.gameObject.GetComponent<Rigidbody2D>().velocity.y, 0f);
            }
            else if (transitionDirection == "Up")
            {
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(3f * player.gameObject.transform.localScale.x, 4f, 0f);
            }

            if (Vector3.Distance(camera.gameObject.transform.position, cameraPos) < 0.01f)
            {
                movingCamera = false;
                foundPosition = false;
                player.gameRunning = true;
                player.changingRooms = false;
            }
        }
    }

    public void UpdateRoom(string direction)
    {
        transitionDirection = direction;
        movingCamera = true;
    }
}
