using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject levelMenu;

    public GameObject levelButtons;

    public GameObject leftButton;
    public GameObject rightButton;

    public int currentLevel;

    private bool movingButtons;

    public float buttonMoveSpeed;
    public bool movingLeft;
    public bool movingRight;
    private Vector3 buttonTargetPos;

    public Image fadeScreen;
    public float fadeSpeed;

    public bool resetLevels;
    public bool unlockAllLevels;

    public AudioSource enterLevelSound;
    public AudioSource arrowButtonSound;
    public AudioSource genericButtonSound;

    public Image muteIcon;
    public bool muted;
    public Sprite mutedSprite;
    public Sprite unMutedSprite;

    private float oldWidth;
    private float newWidth;
    private bool pauseAnim;
    private Vector3 oldButtonPos;

    void Start()
    {
        oldWidth = Display.main.systemWidth;
        newWidth = Display.main.systemWidth;

        OpenStartMenu();

        UnlockFirstLevel();

        fadeScreen.gameObject.SetActive(false);

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

    void Update()
    {
        newWidth = Display.main.systemWidth;

        if (movingButtons && pauseAnim && newWidth == oldWidth)
        {
            movingButtons = false;
            movingLeft = false;
            movingRight = false;
            levelButtons.transform.localPosition = new Vector3(-1200 * (currentLevel - 1), 0f, 0f);
        }

        pauseAnim = newWidth != oldWidth;

        oldWidth = newWidth;

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

        if (movingLeft && !pauseAnim)
        {
            levelButtons.transform.localPosition = Vector3.MoveTowards(levelButtons.transform.localPosition, buttonTargetPos, buttonMoveSpeed * 100f * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(levelButtons.transform.localPosition, buttonTargetPos)) < 0.01f)
            {
                movingLeft = false;
            }
        }

        if (movingRight && !pauseAnim)
        {
            levelButtons.transform.localPosition = Vector3.MoveTowards(levelButtons.transform.localPosition, buttonTargetPos, buttonMoveSpeed * 100f * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(levelButtons.transform.localPosition, buttonTargetPos)) < 0.01f)
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
    }

    public void OpenLevelMenu()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void Left()
    {
        currentLevel--;
        oldButtonPos = levelButtons.transform.localPosition;
        buttonTargetPos = new Vector3(-1200 * (currentLevel - 1), 0f, 0f);
        movingLeft = true;
    }

    public void Right()
    {
        currentLevel++;
        oldButtonPos = levelButtons.transform.localPosition;
        buttonTargetPos = new Vector3(-1200 * (currentLevel - 1), 0f, 0f);
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

        while (currentColor.a < 0.99f)
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
        if (!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetInt("Muted", 0);
        }

        currentLevel = 1;
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
        enterLevelSound.Stop();
        arrowButtonSound.Stop();
        genericButtonSound.Stop();
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
        levelButtons.transform.localPosition = new Vector3(0f, 0f, 0f);
        PlayerPrefs.DeleteAll();
        UnlockFirstLevel();
    }
}
