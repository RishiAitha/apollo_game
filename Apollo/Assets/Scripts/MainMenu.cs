using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelMenu;

    void Start()
    {
        OpenStartMenu();

        UnlockFirstLevel();
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
