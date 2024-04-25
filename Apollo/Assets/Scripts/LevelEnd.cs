using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string nextLevel;

    public int worldToUnlock;

    public void EndLevel()
    {
        PlayerPrefs.SetInt(nextLevel, 1);

        if (worldToUnlock != 0)
        {
            PlayerPrefs.SetInt(worldToUnlock.ToString(), 1);
        }

        SceneManager.LoadScene(nextLevel);
    }
}
