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

    private SpriteRenderer mySR;
    public Sprite sprite1;
    public Sprite sprite2;

    public GameObject lightObj;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mySR = GetComponent<SpriteRenderer>();
        mySR.sprite = sprite1;
        lightObj.SetActive(false);
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
            player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            player.respawning = false;
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void EndLevel()
    {
        PlayerPrefs.SetInt(currentLevel, 0);
        PlayerPrefs.SetInt(nextLevel, 0);

        mySR.sprite = sprite2;
        lightObj.SetActive(true);

        player.respawning = true;

        endingCounter = endingTime;
        ending = true;
    }
}
