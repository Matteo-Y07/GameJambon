using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Variables

    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float maxFallSpeed = 8.0f;
    public float maxTimerGrab = 1f;
    public float coyoteTime = 0.12f;
    public float coyoteTimer = 0f;
    public float jumpBufferTime = 0.12f;
    public float jumpBufferTimer = 0f;
    public float jumpMultiplier = 0.5f;

    // Références
    public Rigidbody2D rb;
    public Transform GroundCheckLeft;
    public Transform GroundCheckRight;
    public Transform WallCheckLeft;
    public Transform WallCheckRight;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Camera PlayerCamera;
    public SpriteRenderer PlayerSpriteRenderer;

    // Variables d'état
    public bool isGrounded= false;
    public bool isTouchingWallLeft;
    public bool isTouchingWallRight;
    public bool hasJump = true;
    public bool grab = false;
    public bool canGrab = true;
    public bool isWallJumping = false;

    // Dash variables
    public float dashPower = 20.0f;
    public Vector2 dashDirection = new Vector2();
    public bool isDashing = false;
    public bool hasDash = true;
    public float gravity = 2.5f;
    
    // Wall jump variables
    public float wallJumpImpulseTime = 0.15f; // durée de l'impulsion
    public float wallJumpImpulseX = 4f; // direction forcée
    public float wallJumpTimer = 0f; // timer actif

    // Controllers
    PlayerMovement_Controller movement;
    PlayerJump_Controller jump;
    PlayerDash_Controller dash;
    PlayerWall_Controller wall;

    private void Awake()
    {
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>();

        movement = new PlayerMovement_Controller(this);
        jump = new PlayerJump_Controller(this);
        dash = new PlayerDash_Controller(this);
        wall = new PlayerWall_Controller(this);
    }

    void Update()
    {
        CheckGround();
        CheckWalls();

        HandleCoyoteTime();
        HandleJumpBuffer();

        movement.Handle();
        jump.Handle();
        dash.Handle();
        wall.Handle();

        ApplyFallGravity();
        LimitFallSpeed();
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    void HandleJumpBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;
    }
    
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapArea(GroundCheckLeft.position, GroundCheckRight.position, groundLayer);
        if (isGrounded){
            hasJump = true;
            hasDash = true;
        }
    }

    void CheckWalls()
    {
        isTouchingWallLeft = Physics2D.OverlapArea(WallCheckLeft.position,WallCheckLeft.position + Vector3.right * 0.1f, wallLayer);
        isTouchingWallRight = Physics2D.OverlapArea(WallCheckRight.position,WallCheckRight.position + Vector3.left * 0.1f, wallLayer);
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed && !isDashing && !grab) {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }

    void ApplyFallGravity()
    {
        if (grab || isDashing) return; // 👈 sort immédiatement si grab ou dash

        if (rb.velocity.y < 0f)
            rb.gravityScale = gravity * 1.8f;
        else if (rb.velocity.y > 0f && (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.C)))
            rb.gravityScale = gravity * 1.3f;
        else
            rb.gravityScale = gravity;
    }

}

