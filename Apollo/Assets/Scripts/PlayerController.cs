using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;

    public BoxCollider2D groundCheckBox;
    public LayerMask groundLayer;

    public float coyoteTime;
    private float coyoteTimeCounter;

    public bool doubleJump;

    public Animator playerAnimator;
    public SpriteRenderer mySR;

    public Material[] playerMaterials;

    public float playerSpeed;
    public float jumpSpeed;

    private bool facingRight;

    public float wallSlideSpeed;

    public Vector3 wallJumpSpeed;
    public float wallJumpDirection;
    public float wallJumpTime;

    public float wallJumpCoyoteTime;
    private float wallJumpCoyoteTimeCounter;

    private bool wallJumping;

    public BoxCollider2D wallCheckBox;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        facingRight = true;
    }

    void Update()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
            doubleJump = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

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
                if (!facingRight)
                {
                    Turn();
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRB.velocity = new Vector3(-playerSpeed, myRB.velocity.y, 0f);
                if (facingRight)
                {
                    Turn();
                }
            }
            else
            {
                myRB.velocity = new Vector3(0f, myRB.velocity.y, 0f);
            }
        }

        if (Input.GetButtonDown("Jump") && (coyoteTimeCounter > 0f || doubleJump))
        {
            myRB.velocity = new Vector3(myRB.velocity.x, jumpSpeed, 0f);
            coyoteTimeCounter = 0f;
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCoyoteTimeCounter > 0f)
        {
            wallJumping = true;
            myRB.velocity = new Vector3(wallJumpDirection * wallJumpSpeed.x, wallJumpSpeed.y, 0f);
            wallJumpCoyoteTimeCounter = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                Turn();
            }

            Invoke("StopWallJumping", wallJumpTime);
        }

        playerAnimator.SetBool("OnGround", coyoteTimeCounter > 0f);
        playerAnimator.SetFloat("PlayerSpeed", Mathf.Abs(myRB.velocity.x));

        if (doubleJump)
        {
            mySR.material = playerMaterials[1];
        }
        else
        {
            mySR.material = playerMaterials[0];
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

    private void Turn()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
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
    }
}
