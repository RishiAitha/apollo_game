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

    private Image fadeScreen;
    public float fadeSpeed;
    public bool fadeInFinished;
    public bool fadeOutFinished;

    public bool credits;
    public float creditsTime;
    public float creditsTimeCounter;

    public AudioSource music;
    public float volumeFadeSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mySR = GetComponent<SpriteRenderer>();
        mySR.sprite = sprite1;
        lightObj.SetActive(false);
        fadeScreen = GameObject.FindGameObjectWithTag("Fade Screen").GetComponent<Image>();
        fadeScreen.color = new Color(0f, 0f, 0f, 1f);
        credits = SceneManager.GetActiveScene().name == "Credits";
        creditsTimeCounter = creditsTime;
    }

    void Update()
    {
        if (credits)
        {
            creditsTimeCounter -= Time.deltaTime;
            if (creditsTimeCounter <= 0f)
            {
                fadeScreen.gameObject.SetActive(true);
                ending = true;
            }
        }

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
            if (!credits)
            {
                Vector3 playerVel = player.gameObject.GetComponent<Rigidbody2D>().velocity;
                player.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(endingSpeed, playerVel.y, 0f);
            }

            Color currentColor = fadeScreen.color;
            currentColor.a = Mathf.Lerp(currentColor.a, 1f, fadeSpeed * Time.deltaTime);
            fadeScreen.color = currentColor;

            if (credits)
            {
                music.volume -= volumeFadeSpeed * Time.deltaTime;
            }

            if (fadeScreen.color.a >= 0.99f)
            {
                fadeOutFinished = true;
            }
        }
        else if (ending)
        {
            if (!credits)
            {
                player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                player.respawning = false;
            }
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void EndLevel(bool loadCredits)
    {
        if (loadCredits)
        {
            nextLevel = "Credits";
        }

        PlayerPrefs.SetInt(currentLevel, 0);
        if (!credits)
        {
            PlayerPrefs.SetInt(nextLevel, 0);
        }

        mySR.sprite = sprite2;
        lightObj.SetActive(true);
        fadeScreen.gameObject.SetActive(true);

        player.levelEndSound.Play();

        player.respawning = true;

        ending = true;
    }
}
