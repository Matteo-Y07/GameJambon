using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float climbSpeed = 2.5f;
    [SerializeField] private float maxFallSpeed = 8.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7.0f;
    [SerializeField] private float jumpMultiplier = 0.5f;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("State")]
    [SerializeField] private float gravity = 2.5f;
    [SerializeField] private float maxTimerGrab = 7.0f;

    [Header("Dash")]
    [SerializeField] private float dashPower = 15.0f;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpImpulseTime = 0.15f;
    [SerializeField] private float wallJumpImpulseX = 4.5f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckLeft;
    [SerializeField] private Transform groundCheckRight;
    [SerializeField] private Transform wallCheckLeftBottom;
    [SerializeField] private Transform wallCheckRightBottom;
    [SerializeField] private Transform wallCheckLeftTop;
    [SerializeField] private Transform wallCheckRightTop;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Runtime State")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWallLeft;
    [SerializeField] private bool isTouchingWallRight;

    public bool hasJump = true;
    public bool grab = false;
    public bool canGrab = true;
    public bool isWallJumping = false;

    public bool isDashing = false;
    public bool hasDash = true;

    public float coyoteTimer = 0f;
    public float jumpBufferTimer = 0f;

    PlayerMovement_Controller movement;
    PlayerJump_Controller jump;
    PlayerDash_Controller dash;
    PlayerWall_Controller wall;

    void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

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
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapArea(
            groundCheckLeft.position,
            groundCheckRight.position,
            groundLayer
        );

        if (isGrounded)
        {
            hasJump = true;
            hasDash = true;
        }
    }

    void CheckWalls()
    {
        bool leftTop = Physics2D.OverlapArea(wallCheckLeftTop.position, wallCheckLeftTop.position + Vector3.right * 0.1f, wallLayer);
        bool leftBottom = Physics2D.OverlapArea(wallCheckLeftBottom.position, wallCheckLeftBottom.position + Vector3.right * 0.1f, wallLayer);

        bool rightTop = Physics2D.OverlapArea(wallCheckRightTop.position, wallCheckRightTop.position + Vector3.left * 0.1f, wallLayer);
        bool rightBottom = Physics2D.OverlapArea(wallCheckRightBottom.position, wallCheckRightBottom.position + Vector3.left * 0.1f, wallLayer);

        isTouchingWallLeft = leftTop || leftBottom;
        isTouchingWallRight = rightTop || rightBottom;
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed && !isDashing && !grab)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }

    void ApplyFallGravity()
    {
        if (grab || isDashing) return;

        if (rb.velocity.y < 0f)
            rb.gravityScale = gravity * 1.8f;
        else if (rb.velocity.y > 0f && !Input.GetButton("Jump"))
            rb.gravityScale = gravity * 1.3f;
        else
            rb.gravityScale = gravity;
    }
}