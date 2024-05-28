using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private LevelManager level;
    private PlayerController player;

    public AudioSource genericButtonSound;

    public Image muteIcon;
    public bool muted;
    public Sprite mutedSprite;
    public Sprite unMutedSprite;

    void Start()
    {
        level = FindObjectOfType<LevelManager>();
        player = FindObjectOfType<PlayerController>();

        muted = PlayerPrefs.GetInt("Muted") == 1;
        if (muted)
        {
            muteIcon.sprite = mutedSprite;
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }
        else
        {
            muteIcon.sprite = unMutedSprite;
        }
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 0);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RespawnCharacter()
    {
        player.transitionImmunityTimeCounter = 0f;
        Time.timeScale = 1f;
        player.CallKillPlayer(false);
        level.UnPause();
    }

    public void ToggleMute()
    {
        if (muted)
        {
            muted = false;
            muteIcon.sprite = unMutedSprite;
            PlayerPrefs.SetInt("Muted", 0);
            StopQueuedSounds();
            AudioListener.pause = false;
            AudioListener.volume = 1f;
        }
        else
        {
            muted = true;
            muteIcon.sprite = mutedSprite;
            PlayerPrefs.SetInt("Muted", 1);
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }
    }

    public void StopQueuedSounds()
    {
        player.elementEnter.Stop();
        player.elementExit.Stop();
        player.crystalCollect.Stop();
        player.checkpointCollect.Stop();
        player.levelEndSound.Stop();
        player.jumpSound.Stop();
        player.slideSound.Stop();
        player.deathSound.Stop();
        player.walkSound.Stop();
        player.dialogueSound.Stop();
        player.dialogueCloseSound.Stop();
        genericButtonSound.Stop();
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
