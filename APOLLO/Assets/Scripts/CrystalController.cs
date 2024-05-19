using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    private PlayerController player;

    public int CrystalID;

    private bool crystalActive;

    public GameObject controlIndicator;

    public CrystalDoorController door;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        door = FindObjectOfType<CrystalDoorController>();

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
        StartCoroutine("CollectCoroutine");
    }

    public IEnumerator CollectCoroutine()
    {
        crystalActive = false;
        GetComponent<BoxCollider2D>().enabled = false;

        PlayerPrefs.SetInt("Crystal" + CrystalID, 1);

        if (!player.crystalCollect.isPlaying)
        {
            player.crystalCollect.Play();
        }

        GetComponent<Animator>().Play("Crystal" + CrystalID + "Collect");

        yield return new WaitForSeconds(3f);

        if (door != null)
        {
            door.CheckCrystals();
        }

        gameObject.SetActive(false);
    }
}
