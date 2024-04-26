using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public string levelName;

    void Start()
    {
        GetComponent<Button>().interactable = (PlayerPrefs.HasKey(levelName) && PlayerPrefs.GetInt(levelName) >= 0);
    }
}
