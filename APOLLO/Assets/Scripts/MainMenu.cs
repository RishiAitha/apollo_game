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

    public float buttonMoveTime;
    private float leftCounter;
    private float rightCounter;
    private float lerpCounter;
    private Vector3 buttonTargetPos;
    private Vector3 tilesTargetPos;

    public bool resetLevels;
    public bool unlockAllLevels;

    void Start()
    {
        OpenStartMenu();

        UnlockFirstLevel();

        currentLevel = 1;
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

        if (leftCounter > 0f || rightCounter > 0f)
        {
            movingButtons = true;
        }
        else
        {
            movingButtons = false;
        }

        if (leftCounter > 0f)
        {
            levelButtons.transform.position = Vector3.Lerp(levelButtons.transform.position, buttonTargetPos, lerpCounter / buttonMoveTime);
            levelTiles.transform.position = Vector3.Lerp(levelTiles.transform.position, tilesTargetPos, lerpCounter / buttonMoveTime);
            leftCounter -= Time.deltaTime;
            lerpCounter += Time.deltaTime;
        }

        if (rightCounter > 0f)
        {
            levelButtons.transform.position = Vector3.Lerp(levelButtons.transform.position, buttonTargetPos, lerpCounter / buttonMoveTime);
            levelTiles.transform.position = Vector3.Lerp(levelTiles.transform.position, tilesTargetPos, lerpCounter / buttonMoveTime);
            rightCounter -= Time.deltaTime;
            lerpCounter += Time.deltaTime;
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
    }

    public void OpenLevelMenu()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void Left()
    {
        currentLevel--;
        buttonTargetPos = new Vector3(levelButtons.transform.position.x + 1200f, levelButtons.transform.position.y, 0f);
        tilesTargetPos = new Vector3(levelTiles.transform.position.x + 12f, levelTiles.transform.position.y, 0f);
        lerpCounter = 0f;
        leftCounter = buttonMoveTime;
    }

    public void Right()
    {
        currentLevel++;
        buttonTargetPos = new Vector3(levelButtons.transform.position.x - 1200f, levelButtons.transform.position.y, 0f);
        tilesTargetPos = new Vector3(levelTiles.transform.position.x - 12f, levelTiles.transform.position.y, 0f);
        lerpCounter = 0f;
        rightCounter = buttonMoveTime;
    }

    public void LoadLevel(LevelButton button)
    {
        SceneManager.LoadScene(button.levelName);
    }

    public void UnlockFirstLevel()
    {
        if (!PlayerPrefs.HasKey("Level1") || PlayerPrefs.GetInt("Level1") < 0)
        {
            PlayerPrefs.SetInt("Level1", 0);
        }
    }
}
