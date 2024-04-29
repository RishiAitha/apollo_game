using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public LevelManager level;
    public PlayerController player;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitPause()
    {
        level.paused = false;
        player.UnPausePlayer();
    }
}
