using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private PlayerController player;

    public int CrystalID;

    private bool crystalActive;

    public GameObject controlIndicator;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (PlayerPrefs.GetInt("Crystal" + CrystalID) == 1)
        {
            // crystal has been found already
            gameObject.SetActive(false);
        }

        controlIndicator.SetActive(false);
    }

    void Update()
    {
        if (crystalActive && player.IsGrounded())
        {
            controlIndicator.SetActive(true);
        }
        else
        {
            controlIndicator.SetActive(false);
        }

        if (crystalActive && Input.GetKeyDown(KeyCode.E) && player.IsGrounded())
        {
            CollectCrystal();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            crystalActive = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            crystalActive = false;
        }
    }

    void CollectCrystal()
    {
        // idk what to do here
        crystalActive = false;
        PlayerPrefs.SetInt("Crystal" +  CrystalID, 1);
        gameObject.SetActive(false);
    }
}
