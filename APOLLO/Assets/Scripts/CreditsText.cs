using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsText : MonoBehaviour
{
    public void StopAnimation()
    {
        GetComponent<Animator>().SetBool("FinishedCredits", true);
        LevelEnd end = FindObjectOfType<LevelEnd>();
        end.fadeScreen.gameObject.SetActive(true);
        end.ending = true;
    }
}
