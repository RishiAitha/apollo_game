using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;

    public BoxCollider2D groundCheckBox;
    public LayerMask groundLayer;

    public bool isGrounded;

    private KillPlane killPlane;

    public float coyoteTime;
    public float coyoteTimeCounter;

    public float jumpTime;
    public float jumpTimeCounter;

    public float jumpBufferTime;
    public float jumpBufferCounter;

    public bool doubleJump;

    public Animator playerAnimator;
    public SpriteRenderer mySR;

    public Material[] playerMaterials;

    public float playerSpeed;
    public float jumpSpeed;
    public float dashSpeed;

    public float wallSlideSpeed;
    public float wallSlideDeceleration;

    public Vector3 wallJumpSpeed;
    public float wallJumpDirection;
    public float wallJumpTime;

    public float wallJumpCoyoteTime;
    public float wallJumpCoyoteTimeCounter;

    public bool wallJumping;

    public BoxCollider2D wallCheckBox;

    private bool dashing;
    public float dashTime;
    public float dashDelay;

    public float origGravityScale;

    public bool pulling;

    public bool onWall;

    public bool dialogueActive;

    public bool changingRooms;
    public bool inTransition;

    public Vector3 respawnPosition;

    public float deathTime;

    private LevelManager level;

    public bool respawning;

    public float transitionImmunityTime;
    public float transitionImmunityTimeCounter;

    public bool zipping;
    private GameObject currentZip;

    public bool paused;

    private Vector3 storedVel;

    public Light2D playerLight;

    public AudioSource elementEnter;
    public AudioSource elementExit;
    public AudioSource crystalCollect;
    public AudioSource checkpointCollect;
    public AudioSource levelEndSound;
    public AudioSource jumpSound;
    public AudioSource slideSound;
    public AudioSource deathSound;
    public AudioSource walkSound;
    public AudioSource dialogueSound;
    public AudioSource dialogueCloseSound;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        level = FindObjectOfType<LevelManager>();
        killPlane = FindObjectOfType<KillPlane>();
        mySR.material = playerMaterials[0];
        origGravityScale = myRB.gravityScale;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Credits")
        {
            bool deactivateKillPlane = false;
            if (killPlane.overlappingTransitions.Count != 0)
            {
                foreach (GameObject transition in killPlane.overlappingTransitions)
                {
                    if (transition.GetComponent<TransitionController>().vertical)
                    {
                        float leftBound = transition.transform.position.x - (transition.GetComponent<BoxCollider2D>().size.y / 2);
                        float rightBound = transition.transform.position.x + (transition.GetComponent<BoxCollider2D>().size.y / 2);
                        if (leftBound < transform.position.x && rightBound > transform.position.x)
                        {
                            deactivateKillPlane = true;
                        }
                    }
                }
            }

            if (deactivateKillPlane)
            {
                killPlane.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                killPlane.GetComponent<BoxCollider2D>().enabled = true;
            }

            transitionImmunityTimeCounter -= Time.deltaTime;

            if (!dialogueActive && !respawning)
            {
                pulling = false;
                foreach (PullController pull in FindObjectsOfType<PullController>())
                {
                    if (pull.aligning || pull.pulling)
                    {
                        pulling = true;
                    }
                }

                zipping = false;
                foreach (ZipController zip in FindObjectsOfType<ZipController>())
                {
                    if (zip.aligning || zip.pulling)
                    {
                        zipping = true;
                    }
                }

                onWall = OnWall();
                isGrounded = IsGrounded();
                if (isGrounded)
                {
                    coyoteTimeCounter = coyoteTime;
                    if (doubleJump && !Input.GetKey(KeyCode.Escape))
                    {
                        elementExit.Play();
                    }
                    doubleJump = false;
                }
                else
                {
                    coyoteTimeCounter -= Time.deltaTime;
                }

                if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !changingRooms && (coyoteTimeCounter > 0f || doubleJump || wallJumpCoyoteTimeCounter > 0f))
                {
                    jumpBufferCounter = jumpBufferTime;
                    jumpTimeCounter = jumpTime;
                }

                if (jumpTimeCounter > 0f && (Input.GetButton("Jump") || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !changingRooms)
                {
                    jumpBufferCounter = jumpBufferTime;
                    jumpTimeCounter -= Time.deltaTime;
                }

                if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    jumpTimeCounter = 0f;
                }

                jumpBufferCounter -= Time.deltaTime;

                if (!dashing && !pulling && !zipping)
                {
                    if (OnWall() && jumpTimeCounter <= 0f && coyoteTimeCounter <= 0f && Input.GetAxisRaw("Horizontal") != 0f && !level.paused)
                    {
                        // we are wall sliding
                        jumpTimeCounter = 0f;
                        float currentWallSlideSpeed = myRB.velocity.y - (wallSlideDeceleration * Time.deltaTime);
                        if (currentWallSlideSpeed < -wallSlideSpeed)
                        {
                            currentWallSlideSpeed = -wallSlideSpeed;
                        }
                        myRB.velocity = new Vector3(myRB.velocity.x, currentWallSlideSpeed, 0f);
                        wallJumpDirection = -transform.localScale.x;
                        wallJumpCoyoteTimeCounter = wallJumpCoyoteTime;

                        if (!slideSound.isPlaying && !Input.GetKey(KeyCode.Escape))
                        {
                            slideSound.Play();
                        }
                    }
                    else
                    {
                        slideSound.Stop();
                        wallJumpCoyoteTimeCounter -= Time.deltaTime;
                    }

                    if (!inTransition && !changingRooms && !wallJumping && !level.paused)
                    {
                        if (Input.GetAxisRaw("Horizontal") > 0f)
                        {
                            myRB.velocity = new Vector3(playerSpeed, myRB.velocity.y, 0f);
                            transform.localScale = new Vector3(1f, 1f, 1f);
                            if (isGrounded && !pulling && !zipping)
                            {
                                if (!walkSound.isPlaying && !Input.GetKey(KeyCode.Escape))
                                {
                                    walkSound.Play();
                                }
                            }
                            else
                            {
                                if (walkSound.isPlaying)
                                {
                                    walkSound.Stop();
                                }
                            }
                        }
                        else if (Input.GetAxisRaw("Horizontal") < 0f)
                        {
                            myRB.velocity = new Vector3(-playerSpeed, myRB.velocity.y, 0f);
                            transform.localScale = new Vector3(-1f, 1f, 1f);
                            if (isGrounded && !pulling && !zipping)
                            {
                                if (!walkSound.isPlaying && !Input.GetKey(KeyCode.Escape))
                                {
                                    walkSound.Play();
                                }
                            }
                            else
                            {
                                if (walkSound.isPlaying)
                                {
                                    walkSound.Stop();
                                }
                            }
                        }
                        else
                        {
                            myRB.velocity = new Vector3(0f, myRB.velocity.y, 0f);
                            if (walkSound.isPlaying)
                            {
                                walkSound.Stop();
                            }
                        }
                    }

                    if (changingRooms)
                    {
                        if (isGrounded && Mathf.Abs(myRB.velocity.x) > 0.01f)
                        {
                            if (!walkSound.isPlaying && !Input.GetKey(KeyCode.Escape))
                            {
                                walkSound.Play();
                            }
                        }
                        else
                        {
                            if (walkSound.isPlaying)
                            {
                                walkSound.Stop();
                            }
                        }
                    }

                    if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && wallJumpCoyoteTimeCounter > 0f && !level.paused)
                    {
                        jumpBufferCounter = 0f;
                        wallJumping = true;
                        myRB.velocity = new Vector3(wallJumpDirection * wallJumpSpeed.x, wallJumpSpeed.y, 0f);
                        wallJumpCoyoteTimeCounter = 0f;

                        if (!jumpSound.isPlaying && !Input.GetKey(KeyCode.Escape))
                        {
                            jumpSound.Play();
                        }

                        if (transform.localScale.x != wallJumpDirection)
                        {
                            transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
                        }

                        Invoke("StopWallJumping", wallJumpTime);
                    }
                }

                if ((!isGrounded || level.paused) && walkSound.isPlaying)
                {
                    walkSound.Stop();
                }

                if (level.paused && slideSound.isPlaying)
                {
                    slideSound.Stop();
                }

                if (!dashing)
                {
                    if (jumpBufferCounter > 0f && !wallJumping && !level.paused)
                    {
                        jumpBufferCounter = 0f;

                        if (doubleJump)
                        {
                            if (!Input.GetKey(KeyCode.Escape))
                            {
                                elementExit.Play();
                            }
                        }
                        else
                        {
                            if (!jumpSound.isPlaying && !elementExit.isPlaying && !Input.GetKey(KeyCode.Escape))
                            {
                                jumpSound.Play();
                            }
                        }
                        myRB.velocity = new Vector3(myRB.velocity.x, jumpSpeed, 0f);
                        coyoteTimeCounter = 0f;
                        doubleJump = false;
                    }
                }

                if (currentZip != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (!Input.GetKey(KeyCode.Escape))
                        {
                            elementExit.Play();
                        }
                        currentZip.GetComponentInParent<ZipController>().Zip();
                        doubleJump = false;
                    }
                }

                if (pulling)
                {
                    mySR.material = playerMaterials[3];
                }
                else if (zipping)
                {
                    mySR.material = playerMaterials[4];
                }
                else if (doubleJump)
                {
                    mySR.material = playerMaterials[1];
                }
                else if (dashing)
                {
                    mySR.material = playerMaterials[2];
                }
                else if (currentZip != null && !currentZip.GetComponent<ZipPointController>().cooldown)
                {
                    mySR.material = playerMaterials[5];
                }
                else
                {
                    mySR.material = playerMaterials[0];
                }
            }
            else if (!respawning)
            {
                myRB.velocity = new Vector3(0f, 0f, 0f);
            }

            if (dialogueActive && walkSound.isPlaying)
            {
                walkSound.Stop();
            } 

            playerLight.color = mySR.material.color;
            playerAnimator.SetBool("OnGround", (IsGrounded() || myRB.velocity.y == 0));
            playerAnimator.SetFloat("PlayerSpeed", Mathf.Abs(myRB.velocity.x));
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckBox.transform.position, groundCheckBox.size, 0f, groundLayer);
    }

    public bool OnWall()
    {
        return Physics2D.OverlapBox(wallCheckBox.transform.position, wallCheckBox.size, 0f, groundLayer);
    }

    private void StopWallJumping()
    {
        wallJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boost")
        {
            if (!respawning)
            {
                doubleJump = true;
                jumpTimeCounter = 0f;
                jumpBufferCounter = 0f;

                other.GetComponent<BoostController>().Cooldown();

                if (!Input.GetKey(KeyCode.Escape))
                {
                    elementEnter.Play();
                }
            }
        }

        if (other.gameObject.tag == "Dash")
        {
            if (!respawning)
            {
                other.GetComponent<DashController>().Cooldown();
                StartCoroutine("Dash", other);

                if (!Input.GetKey(KeyCode.Escape))
                {
                    elementExit.Play();
                }
            }
        }

        if (other.gameObject.tag == "Pull Start")
        {
            if (!respawning)
            {
                other.GetComponentInParent<PullController>().Pull();

                if (!Input.GetKey(KeyCode.Escape))
                {
                    elementEnter.Play();
                }
            }
        }

        if (other.gameObject.tag == "Portal Start")
        {
            if (!respawning)
            {
                other.GetComponentInParent<PortalController>().Teleport(myRB.velocity);

                if (!Input.GetKey(KeyCode.Escape))
                {
                    elementExit.Play();
                }
            }
        }

        if (other.gameObject.tag == "Zip Point")
        {
            if (!respawning)
            {
                currentZip = other.gameObject;
                currentZip.GetComponentInParent<ZipController>().SetStart(currentZip);
            }
        }

        if (other.gameObject.tag == "Transition")
        {
            transitionImmunityTimeCounter = transitionImmunityTime;
            inTransition = true;
        }

        if (other.gameObject.tag == "Level End")
        {
            other.GetComponent<LevelEnd>().EndLevel(false);
        }

        if (other.gameObject.tag == "Kill Plane" && !changingRooms)
        {
            StartCoroutine("KillPlayer", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Zip Point")
        {
            currentZip = null;
        }

        if (other.gameObject.tag == "Transition")
        {
            wallJumping = false;
            wallJumpCoyoteTimeCounter = 0f;
            inTransition = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Hazard")
        {
            StartCoroutine("KillPlayer", true);
        }
    }

    public IEnumerator Dash(Collider2D other)
    {
        yield return new WaitForSeconds(dashDelay);

        dashing = true;

        myRB.velocity = dashSpeed * Vector3.Normalize(other.transform.up);
        myRB.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime);

        myRB.velocity = new Vector3(myRB.velocity.x, myRB.velocity.y / 2, 0f);

        myRB.gravityScale = origGravityScale;
        dashing = false;
    }

    public void CallKillPlayer(bool hazard)
    {
        StartCoroutine("KillPlayer", hazard);
    }

    public IEnumerator KillPlayer(bool hazard)
    {
        if (!respawning && transitionImmunityTimeCounter <= 0f)
        {
            respawning = true;

            playerAnimator.SetBool("Hurt", true);

            if (!Input.GetKey(KeyCode.Escape))
            {
                deathSound.Play();
            }

            if (hazard)
            {
                myRB.velocity = new Vector3(-3f * transform.localScale.x, 5f, 0f);
            }
            else
            {
                myRB.velocity = Vector3.zero;
            }

            yield return new WaitForSeconds(deathTime);

            level.ResetCamera();
            ResetVars();

            myRB.velocity = Vector3.zero;
            transform.position = respawnPosition;
            playerAnimator.SetBool("Hurt", false);

            respawning = false;
        }
    }

    public void ResetVars()
    {
        myRB.gravityScale = origGravityScale;
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
        doubleJump = false;
        mySR.material = playerMaterials[0];
        wallJumpCoyoteTimeCounter = 0f;
        wallJumping = false;
        dashing = false;
        pulling = false;
        onWall = false;
        playerAnimator.speed = 1f;
        currentZip = null;

        foreach (PullController pull in FindObjectsOfType<PullController>())
        {
            pull.aligning = false;
            pull.pulling = false;
        }

        foreach (ZipController zip in FindObjectsOfType<ZipController>())
        {
            zip.aligning = false;
            zip.pulling = false;
        }
    }

    public void PausePlayer()
    {
        storedVel = myRB.velocity;
    }

    public void UnPausePlayer()
    {
        myRB.velocity = storedVel;
    }
}
