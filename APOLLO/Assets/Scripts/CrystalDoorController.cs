using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDoorController : MonoBehaviour
{
    public GameObject door;
    public bool resetCrystals;

    void Start()
    {
        CheckCrystals();
    }

    void Update()
    {
        if (resetCrystals)
        {
            resetCrystals = false;
            for (int i = 1; i < 6; i++)
            {
                PlayerPrefs.SetInt("Crystal" + i, 0);
            }
            door.SetActive(true);
        }
    }

    public void CheckCrystals()
    {
        bool missingCrystals = false;

        for (int i = 1; i < 6; i++)
        {
            if (PlayerPrefs.GetInt("Crystal" + i) != 1)
            {
                missingCrystals = true;
            }
        }

        if (!missingCrystals)
        {
            door.SetActive(false);
        }
        else
        {
            door.SetActive(true);
        }
    }
}
