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

    private PlayerController player;

    private bool NPCActive;

    public GameObject controlIndicator;

    public List<dialoguePair> dialogue;

    public int currentLine = 0;

    public GameObject dialogueCanvas;

    public Image dialogueImage;

    public TextMeshProUGUI dialogueText;

    public Sprite playerSprite;
    public Sprite NPCSprite;

    public string NPCType;

    public static float textDelay = 0.01f;
    public bool displayingText;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        if (NPCType != "Rocket")
        {
            Animator anim = GetComponent<Animator>();
            anim.Play(NPCType + "Idle", 0, Random.Range(0f, 1f));
        }
    }

    void Update()
    {
        if (NPCActive && player.IsGrounded())
        {
            controlIndicator.SetActive(true);
        }
        else
        {
            controlIndicator.SetActive(false);
        }

        if (NPCActive && Input.GetKeyDown(KeyCode.E) && player.IsGrounded() && !displayingText)
        {
            if (currentLine == 0)
            {
                player.dialogueActive = true;
                dialogueCanvas.SetActive(true);
            }

            if (currentLine < dialogue.Count)
            {
                DisplayLine(dialogue[currentLine].text, dialogue[currentLine].isPlayer);
                
                if (NPCType == "Rocket" && dialogue[currentLine].text == "PASSENGER LOADED")
                {
                    player.mySR.enabled = false;
                }

                currentLine++;
                player.dialogueSound.Play();

            }
            else
            {
                currentLine = 0;
                dialogueCanvas.SetActive(false);
                if (NPCType != "Rocket")
                {
                    player.dialogueCloseSound.Play();
                    player.dialogueActive = false;
                }
                else
                {
                    NPCActive = false;
                    player.mySR.enabled = false;
                    controlIndicator.SetActive(false);
                    transform.parent.gameObject.GetComponent<Animator>().SetBool("GameFinished", true);
                    LevelEnd end = FindObjectOfType<LevelEnd>();
                    end.endingSpeed = 0f;
                    end.fadeSpeed = 0.5f;
                    end.EndLevel(true);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            NPCActive = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
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

        StartCoroutine("DisplayText", text);
    }

    public IEnumerator DisplayText(string text)
    {
        displayingText = true;
        dialogueText.text = "";
        int currentChar = 0;
        while (dialogueText.text != text)
        {
            dialogueText.text = dialogueText.text + text[currentChar];
            currentChar++;
            yield return new WaitForSeconds(textDelay);
        }
        displayingText = false;
    }
}
