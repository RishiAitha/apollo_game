using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string currentLevel;
    public string nextLevel;

    public void EndLevel()
    {
        PlayerPrefs.SetInt(currentLevel, 0);
        PlayerPrefs.SetInt(nextLevel, 0);

        SceneManager.LoadScene(nextLevel);
    }
}
