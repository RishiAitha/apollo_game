using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    private PlayerController player;

    public string currentLevel;
    public string nextLevel;

    public float endingSpeed;
    private bool ending;

    private SpriteRenderer mySR;
    public Sprite sprite1;
    public Sprite sprite2;

    public GameObject lightObj;

    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadeInFinished;
    public bool fadeOutFinished;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mySR = GetComponent<SpriteRenderer>();
        mySR.sprite = sprite1;
        lightObj.SetActive(false);
        fadeScreen = GameObject.FindGameObjectWithTag("Fade Screen").GetComponent<Image>();
        fadeScreen.color = new Color(0f, 0f, 0f, 0.95f);
    }

    void Update()
    {
        if (!fadeInFinished)
        {
            Color currentColor = fadeScreen.color;
            currentColor.a = Mathf.Lerp(currentColor.a, 0f, fadeSpeed * Time.deltaTime);
            fadeScreen.color = currentColor;

            if (fadeScreen.color.a <= 0.01f)
            {
                fadeInFinished = true;
                fadeScreen.gameObject.SetActive(false);
            }
        }

        if (ending && !fadeOutFinished)
        {
            Vector3 playerVel = player.gameObject.GetComponent<Rigidbody2D>().velocity;
            player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(endingSpeed, playerVel.y, 0f);

            Color currentColor = fadeScreen.color;
            currentColor.a = Mathf.Lerp(currentColor.a, 1f, fadeSpeed * Time.deltaTime);
            fadeScreen.color = currentColor;

            if (fadeScreen.color.a >= 0.95f)
            {
                fadeOutFinished = true;
            }
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
        fadeScreen.gameObject.SetActive(true);

        player.levelEndSound.Play();

        player.respawning = true;

        ending = true;
    }
}
