using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelMenu;

    public GameObject levelButtons;
    public GameObject levelTiles;

    public GameObject leftButton;
    public GameObject rightButton;

    public int currentLevel;

    private bool movingButtons;

    public float buttonMoveSpeed;
    public bool movingLeft;
    public bool movingRight;
    private Vector3 buttonTargetPos;
    private Vector3 tilesTargetPos;

    public Image fadeScreen;
    public float fadeSpeed;

    public bool resetLevels;
    public bool unlockAllLevels;

    public AudioSource enterLevelSound;
    public AudioSource arrowButtonSound;
    public AudioSource genericButtonSound;

    void Start()
    {
        OpenStartMenu();

        UnlockFirstLevel();

        currentLevel = 1;
        levelTiles.SetActive(false);
    }

    void Update()
    {

        if (!movingButtons)
        {
            if (currentLevel == 1)
            {
                leftButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                leftButton.GetComponent<Button>().interactable = true;
            }

            if (currentLevel == 5 || !PlayerPrefs.HasKey("Level" + (currentLevel + 1)) || PlayerPrefs.GetInt("Level" + (currentLevel + 1)) < 0)
            {
                rightButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                rightButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            leftButton.GetComponent<Button>().interactable = false;
            rightButton.GetComponent<Button>().interactable = false;
        }

        if (movingLeft || movingRight)
        {
            movingButtons = true;
        }
        else
        {
            movingButtons = false;
        }

        if (movingLeft)
        {
            levelButtons.transform.position = Vector3.MoveTowards(levelButtons.transform.position, buttonTargetPos, buttonMoveSpeed * (100f * (Screen.currentResolution.width / 1920f)) * Time.deltaTime);
            levelTiles.transform.position = Vector3.MoveTowards(levelTiles.transform.position, tilesTargetPos, buttonMoveSpeed * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(levelButtons.transform.position, buttonTargetPos)) < 0.01f && Mathf.Abs(Vector3.Distance(levelTiles.transform.position, tilesTargetPos)) < 0.01f)
            {
                movingLeft = false;
            }
        }

        if (movingRight)
        {
            levelButtons.transform.position = Vector3.MoveTowards(levelButtons.transform.position, buttonTargetPos, buttonMoveSpeed * (100f * (Screen.currentResolution.width / 1920f)) * Time.deltaTime);
            levelTiles.transform.position = Vector3.MoveTowards(levelTiles.transform.position, tilesTargetPos, buttonMoveSpeed * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(levelButtons.transform.position, buttonTargetPos)) < 0.01f && Mathf.Abs(Vector3.Distance(levelTiles.transform.position, tilesTargetPos)) < 0.01f)
            {
                movingRight = false;
            }
        }

        if (resetLevels)
        {
            resetLevels = false;

            PlayerPrefs.SetInt("Level1", 0);

            for (int i = 2; i < 6; i++)
            {
                PlayerPrefs.DeleteKey("Level" + i);
            }
        }

        if (unlockAllLevels)
        {
            unlockAllLevels = false;

            for (int i = 1; i < 6; i++)
            {
                PlayerPrefs.SetInt("Level" + i, 0);
            }
        }
    }

    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
        levelTiles.SetActive(false);
    }

    public void OpenLevelMenu()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
        levelTiles.SetActive(true);
    }

    public void Left()
    {
        currentLevel--;
        buttonTargetPos = new Vector3(levelButtons.transform.position.x + (1200f * (Screen.currentResolution.width / 1920f)), levelButtons.transform.position.y, 0f);
        tilesTargetPos = new Vector3(levelTiles.transform.position.x + 12f, levelTiles.transform.position.y, 0f);
        Debug.Log(Screen.currentResolution.width);
        Debug.Log(levelButtons.transform.position);
        Debug.Log(buttonTargetPos);
        movingLeft = true;
    }

    public void Right()
    {
        currentLevel++;
        buttonTargetPos = new Vector3(levelButtons.transform.position.x - (1200f * (Screen.currentResolution.width / 1920f)), levelButtons.transform.position.y, 0f);
        tilesTargetPos = new Vector3(levelTiles.transform.position.x - 12f, levelTiles.transform.position.y, 0f);
        Debug.Log(Screen.currentResolution.width);
        Debug.Log(levelButtons.transform.position);
        Debug.Log(buttonTargetPos);
        movingRight = true;
    }

    public void LoadLevel(LevelButton button)
    {
        StartCoroutine("LoadLevelCoroutine", button);
    }

    public IEnumerator LoadLevelCoroutine(LevelButton button)
    {
        enterLevelSound.Play();

        fadeScreen.gameObject.SetActive(true);

        Color currentColor = fadeScreen.color;

        while (currentColor.a < 0.95f)
        {
            currentColor.a = Mathf.Lerp(currentColor.a, 1f, fadeSpeed * Time.deltaTime);
            fadeScreen.color = currentColor;
            yield return null;
        }

        SceneManager.LoadScene(button.levelName);
    }

    public void UnlockFirstLevel()
    {
        if (!PlayerPrefs.HasKey("Level1") || PlayerPrefs.GetInt("Level1") < 0)
        {
            PlayerPrefs.SetInt("Level1", 0);
        }
    }

    public void PlayArrowButtonSound()
    {
        arrowButtonSound.Play();
    }

    public void PlayGenericButtonSound()
    {
        genericButtonSound.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetSaveData()
    {
        PlayerPrefs.DeleteAll();
        UnlockFirstLevel();
    }
}
