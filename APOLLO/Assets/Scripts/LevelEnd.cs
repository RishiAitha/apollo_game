using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    private PlayerController player;

    public string currentLevel;
    public string nextLevel;

    public float endingTime;
    public float endingSpeed;
    private float endingCounter;
    private bool ending;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (ending && endingCounter >= 0f)
        {
            Vector3 playerVel = player.gameObject.GetComponent<Rigidbody2D>().velocity;
            player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(endingSpeed, playerVel.y, 0f);
            endingCounter -= Time.deltaTime;
        }
        else if (ending)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void EndLevel()
    {
        PlayerPrefs.SetInt(currentLevel, 0);
        PlayerPrefs.SetInt(nextLevel, 0);

        // need to stop player from moving from input

        endingCounter = endingTime;
        ending = true;
    }
}
