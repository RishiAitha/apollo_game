using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentRoomID;
    public GameObject[] rooms;

    public int currentCheckpointID;
    public GameObject[] checkpoints;

    private Camera mainCamera;

    private bool movingCamera;
    private bool foundPosition;

    public float cameraSpeed;

    private PlayerController player;

    public Animator playerAnimator;

    private float vertical;
    private float horizontal;
    private float cameraSize;
    private Vector3 cameraPos;

    private string transitionDirection;

    private Vector3 playerVelocity;

    private bool boostedPlayer;

    public float vertTransitionHorizontalBoost;
    public float vertTransitionVerticalBoost;

    private GameObject currentTransitionObj;

    public bool paused;
    public GameObject pauseMenu;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mainCamera = FindObjectOfType<Camera>();

        transitionDirection = "Right";

        movingCamera = false;
        foundPosition = false;

        currentCheckpointID = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
        player.respawnPosition = checkpoints[currentCheckpointID].transform.position;
        player.transform.position = player.respawnPosition;
        UpdateRoom("None", null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                player.PausePlayer();
            }
            else
            {
                player.UnPausePlayer();
            }
        }

        if (paused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }

        if (movingCamera && !foundPosition)
        {
            if (transitionDirection != "Down")
            {
                player.changingRooms = true;
            }
            BoxCollider2D roomDimensions = rooms[currentRoomID].GetComponent<BoxCollider2D>();

            vertical = roomDimensions.size.y;
            horizontal = roomDimensions.size.x * mainCamera.pixelHeight / mainCamera.pixelWidth;

            cameraSize = Mathf.Max(vertical, horizontal) * 0.5f;

            cameraPos = new Vector3(roomDimensions.gameObject.transform.position.x, roomDimensions.gameObject.transform.position.y, -10f);

            playerVelocity = player.gameObject.GetComponent<Rigidbody2D>().velocity;

            foundPosition = true;

            boostedPlayer = transitionDirection != "Up";
        }

        if (movingCamera && foundPosition)
        {
            mainCamera.gameObject.transform.position = Vector3.Lerp(mainCamera.gameObject.transform.position, cameraPos, cameraSpeed * Time.deltaTime);
            
            mainCamera.orthographicSize = cameraSize;

            if (transitionDirection == "Right" || transitionDirection == "Left")
            {
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(playerVelocity.x * 0.5f, player.gameObject.GetComponent<Rigidbody2D>().velocity.y, 0f);
                playerAnimator.speed = 0.5f;
            }
            else if (transitionDirection == "Up")
            {
                if (!boostedPlayer)
                {
                    if (Vector3.Distance(player.gameObject.transform.position, currentTransitionObj.transform.position) > 0.5f)
                    {
                        player.gameObject.transform.position = Vector3.Lerp(player.gameObject.transform.position, currentTransitionObj.transform.position, 45f * Time.deltaTime);
                    }
                    else
                    {
                        player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(vertTransitionHorizontalBoost * player.gameObject.transform.localScale.x, vertTransitionVerticalBoost, 0f);
                        boostedPlayer = true;
                    }
                }
            }

            if (Vector3.Distance(mainCamera.gameObject.transform.position, cameraPos) < 0.01f)
            {
                if (player.IsGrounded() && boostedPlayer)
                {
                    movingCamera = false;
                    foundPosition = false;
                    player.changingRooms = false;
                    playerAnimator.speed = 1f;
                }
            }
        }
    }

    public void UpdateRoom(string direction, GameObject transitionObj)
    {
        transitionDirection = direction;
        currentTransitionObj = transitionObj;
        movingCamera = true;
    }

    public void ResetCamera()
    {
        transitionDirection = "Reset";
        currentTransitionObj = null;
        currentRoomID = checkpoints[currentCheckpointID].GetComponent<CheckpointController>().roomID;
        movingCamera = true;
        foundPosition = false;
    }
}