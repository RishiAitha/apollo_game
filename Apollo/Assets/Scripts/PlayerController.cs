using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;

    public BoxCollider2D groundCheckBox;
    public LayerMask groundLayer;

    public float coyoteTime;
    private float coyoteTimeCounter;

    public float jumpTime;
    private float jumpTimeCounter;

    public float jumpBufferTime;
    private float jumpBufferCounter;

    public bool doubleJump;

    public Animator playerAnimator;
    public SpriteRenderer mySR;

    public Material[] playerMaterials;

    public float playerSpeed;
    public float jumpSpeed;
    public float dashSpeed;

    public float wallSlideSpeed;

    public Vector3 wallJumpSpeed;
    public float wallJumpDirection;
    public float wallJumpTime;

    public float wallJumpCoyoteTime;
    private float wallJumpCoyoteTimeCounter;

    private bool wallJumping;

    public BoxCollider2D wallCheckBox;

    private bool dashing;
    public float dashTime;
    public float dashDelay;

    public float origGravityScale;

    public bool pulling;

    public bool onWall;

    public bool dialogueActive;

    public bool changingRooms;

    public Vector3 respawnPosition;

    public float deathTime;

    private LevelManager level;

    private bool respawning;

    public float transitionImmunityTime;
    private float transitionImmunityTimeCounter;

    public bool zipping;
    private GameObject currentZip;

    public bool paused;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        level = FindObjectOfType<LevelManager>();
        mySR.material = playerMaterials[0];
        origGravityScale = myRB.gravityScale;
    }

    void Update()
    {
        transitionImmunityTimeCounter -= Time.deltaTime;

        if (!paused && !dialogueActive && !respawning)
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
            if (IsGrounded())
            {
                coyoteTimeCounter = coyoteTime;
                doubleJump = false;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !changingRooms && (coyoteTimeCounter > 0f || doubleJump))
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

            if (!dashing && !pulling)
            {
                if (OnWall() && coyoteTimeCounter <= 0f && Input.GetAxisRaw("Horizontal") != 0f)
                {
                    // we are wall sliding
                    myRB.velocity = new Vector3(myRB.velocity.x, -wallSlideSpeed, 0f);
                    wallJumpDirection = -transform.localScale.x;
                    wallJumpCoyoteTimeCounter = wallJumpCoyoteTime;
                    CancelInvoke("StopWallJumping");
                }
                else
                {
                    wallJumpCoyoteTimeCounter -= Time.deltaTime;
                }

                if (!changingRooms && !wallJumping)
                {
                    if (Input.GetAxisRaw("Horizontal") > 0f)
                    {
                        myRB.velocity = new Vector3(playerSpeed, myRB.velocity.y, 0f);
                        transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else if (Input.GetAxisRaw("Horizontal") < 0f)
                    {
                        myRB.velocity = new Vector3(-playerSpeed, myRB.velocity.y, 0f);
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else
                    {
                        myRB.velocity = new Vector3(0f, myRB.velocity.y, 0f);
                    }
                }

                if (jumpBufferCounter > 0f && wallJumpCoyoteTimeCounter > 0f)
                {
                    jumpBufferCounter = 0f;
                    wallJumping = true;
                    myRB.velocity = new Vector3(wallJumpDirection * wallJumpSpeed.x, wallJumpSpeed.y, 0f);
                    wallJumpCoyoteTimeCounter = 0f;

                    if (transform.localScale.x != wallJumpDirection)
                    {
                        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
                    }

                    Invoke("StopWallJumping", wallJumpTime);
                }
            }

            if (!dashing)
            {
                if (jumpBufferCounter > 0f)
                {
                    jumpBufferCounter = 0f;
                    myRB.velocity = new Vector3(myRB.velocity.x, jumpSpeed, 0f);
                    coyoteTimeCounter = 0f;
                    doubleJump = false;
                }
            }

            if (currentZip != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
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

        playerAnimator.SetBool("OnGround", IsGrounded());
        playerAnimator.SetFloat("PlayerSpeed", Mathf.Abs(myRB.velocity.x));
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
            doubleJump = true;

            other.GetComponent<BoostController>().Cooldown();
        }

        if (other.gameObject.tag == "Dash")
        {
            other.GetComponent<DashController>().Cooldown();
            StartCoroutine("Dash", other);
        }

        if (other.gameObject.tag == "Pull Start")
        {
            other.GetComponentInParent<PullController>().Pull();
        }

        if (other.gameObject.tag == "Portal Start")
        {
            other.GetComponentInParent<PortalController>().Teleport(myRB.velocity);
        }

        if (other.gameObject.tag == "Transition")
        {
            transitionImmunityTimeCounter = transitionImmunityTime;
        }

        if (other.gameObject.tag == "Level End")
        {
            other.GetComponent<LevelEnd>().EndLevel();
        }

        if (other.gameObject.tag == "Hazard")
        {
            StartCoroutine("KillPlayer", true);
        }

        if (other.gameObject.tag == "Kill Plane")
        {
            StartCoroutine("KillPlayer", false);
        }

        if (other.gameObject.tag == "Zip Point")
        {
            currentZip = other.gameObject;
            currentZip.GetComponentInParent<ZipController>().SetStart(currentZip);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Zip Point")
        {
            currentZip = null;
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

    public IEnumerator KillPlayer(bool hazard)
    {
        if (!respawning && transitionImmunityTimeCounter <= 0f)
        {
            respawning = true;

            playerAnimator.SetBool("Hurt", true);

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
    }

    public void PausePlayer()
    {
        myRB.gravityScale = 0f;
        paused = true;
    }

    public void UnPausePlayer()
    {
        myRB.gravityScale = origGravityScale;
        paused = false;
    }
}
