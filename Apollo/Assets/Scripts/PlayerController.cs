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

    public float jumpBufferTime;
    private float jumpBufferCounter;

    public bool doubleJump;

    public Animator playerAnimator;
    public SpriteRenderer mySR;

    public Material[] playerMaterials;

    public float playerSpeed;
    public float jumpSpeed;
    public float dashSpeed;

    private bool facingRight;

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

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        facingRight = true;
        mySR.material = playerMaterials[0];
        origGravityScale = myRB.gravityScale;
    }

    void Update()
    {
        if (!dialogueActive)
        {
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

            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

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

                if (!wallJumping)
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
                if (jumpBufferCounter > 0f && (coyoteTimeCounter > 0f || doubleJump))
                {
                    jumpBufferCounter = 0f;
                    myRB.velocity = new Vector3(myRB.velocity.x, jumpSpeed, 0f);
                    coyoteTimeCounter = 0f;
                    doubleJump = false;
                }
            }

            playerAnimator.SetBool("OnGround", coyoteTimeCounter > 0f);
            playerAnimator.SetFloat("PlayerSpeed", Mathf.Abs(myRB.velocity.x));

            if (pulling)
            {
                mySR.material = playerMaterials[3];
            }
            else if (doubleJump)
            {
                mySR.material = playerMaterials[1];
            }
            else if (dashing)
            {
                mySR.material = playerMaterials[2];
            }
            else
            {
                mySR.material = playerMaterials[0];
            }
        }
        else
        {
            myRB.velocity = new Vector3(0f, 0f, 0f);
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
}
