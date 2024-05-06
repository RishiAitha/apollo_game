using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCController : MonoBehaviour
{
    [System.Serializable]
    public struct dialoguePair
    {
        public string text;
        public bool isPlayer;
    }

    public PlayerController player;

    public bool NPCActive;

    public GameObject controlIndicator;

    public List<dialoguePair> dialogue;

    public int currentLine = 0;

    public GameObject dialogueCanvas;

    public Image dialogueImage;

    public Sprite playerSprite;
    public Sprite NPCSprite;

    public TextMeshProUGUI dialogueText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        Animator anim = GetComponent<Animator>();

        anim.Play("FairyIdle", 0, Random.Range(0f, 1f));
    }

    void Update()
    {
        if (NPCActive && Input.GetKeyDown(KeyCode.E) && player.IsGrounded())
        {
            if (currentLine == 0)
            {
                player.dialogueActive = true;
                dialogueCanvas.SetActive(true);
            }

            if (currentLine < dialogue.Count)
            {
                DisplayLine(dialogue[currentLine].text, dialogue[currentLine].isPlayer);
                currentLine++;
            }
            else
            {
                player.dialogueActive = false;
                currentLine = 0;
                dialogueCanvas.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            controlIndicator.SetActive(true);
            NPCActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            controlIndicator.SetActive(false);
            NPCActive = false;
        }
    }

    void DisplayLine(string text, bool isPlayer)
    {
        if (isPlayer)
        {
            dialogueImage.sprite = playerSprite;
        }
        else
        {
            dialogueImage.sprite = NPCSprite;
        }

        dialogueText.text = text;
    }
}
