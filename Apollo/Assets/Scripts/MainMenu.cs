using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject worldMenu;
    public GameObject[] worlds;

    void Start()
    {
        OpenStartMenu();

        UnlockFirstLevel();
    }

    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
        worldMenu.SetActive(false);
        foreach (GameObject world in worlds) {
            world.SetActive(false);
        }
    }

    public void OpenWorldMenu()
    {
        startMenu.SetActive(false);
        worldMenu.SetActive(true);
        foreach (GameObject world in worlds) {
            world.SetActive(false);
        }
    }

    public void OpenWorld(int worldNum)
    {
        startMenu.SetActive(false);
        worldMenu.SetActive(false);
        foreach (GameObject world in worlds) {
            world.SetActive(false);
        }
        worlds[worldNum - 1].SetActive(true);
    }

    public void LoadLevel(LevelButton button)
    {
        SceneManager.LoadScene(button.levelName);
    }

    public void UnlockFirstLevel()
    {
        if (!PlayerPrefs.HasKey("1") || PlayerPrefs.GetInt("1") != 1)
        {
            PlayerPrefs.SetInt("1", 1);
        }

        if (!PlayerPrefs.HasKey("1-1") || PlayerPrefs.GetInt("1-1") != 1)
        {
            PlayerPrefs.SetInt("1-1", 1);
        }
    }
}
