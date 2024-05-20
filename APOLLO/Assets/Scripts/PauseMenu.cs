using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private LevelManager level;
    private PlayerController player;

    public AudioSource genericButtonSound;

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

    public void RespawnCharacter()
    {
        player.transitionImmunityTimeCounter = 0f;
        Time.timeScale = 1f;
        player.CallKillPlayer(false);
        level.UnPause();
    }

    public void ExitPause()
    {
        level.UnPause();
    }

    public void PlayGenericButtonSound()
    {
        genericButtonSound.Play();
    }
}
