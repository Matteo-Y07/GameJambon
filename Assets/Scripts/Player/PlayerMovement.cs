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
    [SerializeField] private Transform wallCheckLeftTop;
    [SerializeField] private Transform wallCheckRightBottom;
    [SerializeField] private Transform wallCheckRightTop;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer dashTrail;

    [Header("Runtime State")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWallLeft;
    [SerializeField] private bool isTouchingWallRight;

    private bool hasJump = true;
    private bool grab = false;
    private bool canGrab = true;
    private bool isWallJumping = false;
    private bool isDashing = false;
    private bool hasDash = true;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    private PlayerMovement_Controller movement;
    private PlayerJump_Controller jump;
    private PlayerDash_Controller dash;
    private PlayerWall_Controller wall;
    
    private void Awake() 
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = new PlayerMovement_Controller(this);
        jump = new PlayerJump_Controller(this);
        dash = new PlayerDash_Controller(this);
        wall = new PlayerWall_Controller(this);
        dashTrail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        dashTrail.enabled = false;
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
        if (IsGrounded())
            coyoteTimer = GetCoyoteTime();
        else
            coyoteTimer -= Time.deltaTime;
    }

    void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = GetJumpBufferTime();
        else
            jumpBufferTimer -= Time.deltaTime;
    }

    void CheckGround()
    {
        bool grounded = Physics2D.OverlapArea(
            groundCheckLeft.position,
            groundCheckRight.position,
            groundLayer
        );

        SetGrounded(grounded);

        if (grounded)
        {
            SetHasJump(true);
            SetHasDash(true);
        }
    }

    void CheckWalls()
    {
        bool leftTop = Physics2D.OverlapArea(
            wallCheckLeftTop.position,
            wallCheckLeftTop.position + Vector3.right * 0.1f,
            wallLayer
        );

        bool leftBottom = Physics2D.OverlapArea(
            wallCheckLeftBottom.position,
            wallCheckLeftBottom.position + Vector3.right * 0.1f,
            wallLayer
        );

        bool rightTop = Physics2D.OverlapArea(
            wallCheckRightTop.position,
            wallCheckRightTop.position + Vector3.left * 0.1f,
            wallLayer
        );

        bool rightBottom = Physics2D.OverlapArea(
            wallCheckRightBottom.position,
            wallCheckRightBottom.position + Vector3.left * 0.1f,
            wallLayer
        );

        SetWallLeft(leftTop || leftBottom);
        SetWallRight(rightTop || rightBottom);
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -GetMaxFallSpeed() && !IsDashing() && !IsGrabbing())
        {
            rb.velocity = new Vector2(rb.velocity.x, -GetMaxFallSpeed());
        }
    }

    void ApplyFallGravity()
    {
        if (IsGrabbing() || IsDashing())
            return;

        if (rb.velocity.y < 0f)
            rb.gravityScale = GetGravity() * 1.8f;
        else if (rb.velocity.y > 0f && !Input.GetButton("Jump"))
            rb.gravityScale = GetGravity() * 1.3f;
        else
            rb.gravityScale = GetGravity();
    }

    // =========================
    // GETTERS (CONFIG)
    // =========================

    public Rigidbody2D GetRigidbody() => rb;

    public float GetMoveSpeed() => moveSpeed;
    public float GetClimbSpeed() => climbSpeed;
    public float GetMaxFallSpeed() => maxFallSpeed;

    public float GetJumpForce() => jumpForce;
    public float GetJumpMultiplier() => jumpMultiplier;
    public float GetCoyoteTime() => coyoteTime;
    public float GetJumpBufferTime() => jumpBufferTime;

    public float GetGravity() => gravity;
    public float GetMaxTimerGrab() => maxTimerGrab;

    public float GetDashPower() => dashPower;

    public float GetWallJumpImpulseTime() => wallJumpImpulseTime;
    public float GetWallJumpImpulseX() => wallJumpImpulseX;

    public SpriteRenderer GetSpriteRenderer() => spriteRenderer;
    public Camera GetPlayerCamera() => playerCamera;
    public TrailRenderer GetTrail() => dashTrail;

    // =========================
    // GETTERS (STATE)
    // =========================

    public bool IsGrounded() => isGrounded;
    public bool IsTouchingWallLeft() => isTouchingWallLeft;
    public bool IsTouchingWallRight() => isTouchingWallRight;

    // -------------------------
    // WALL CORNER GETTERS
    // -------------------------

    public bool IsTouchingWallLeftTop()
    {
        return Physics2D.OverlapArea(
            wallCheckLeftTop.position,
            wallCheckLeftTop.position,
            wallLayer
        );
    }

    public bool IsTouchingWallLeftBottom()
    {
        return Physics2D.OverlapArea(
            wallCheckLeftBottom.position,
            wallCheckLeftBottom.position + Vector3.right * 0.1f,
            wallLayer
        );
    }

    public bool IsTouchingWallRightTop()
    {
        return Physics2D.OverlapArea(
            wallCheckRightTop.position,
            wallCheckRightTop.position,
            wallLayer
        );
    }

    public bool IsTouchingWallRightBottom()
    {
        return Physics2D.OverlapArea(
            wallCheckRightBottom.position,
            wallCheckRightBottom.position + Vector3.left * 0.1f,
            wallLayer
        );
    }
   

    public bool HasJump() => hasJump;
    public bool HasDash() => hasDash;

    public bool IsDashing() => isDashing;
    public bool IsWallJumping() => isWallJumping;
    public bool IsGrabbing() => grab;
    public bool CanGrab() => canGrab;

    public float GetCoyoteTimer() => coyoteTimer;
    public float GetJumpBufferTimer() => jumpBufferTimer;

    // =========================
    // SETTERS (STATE ONLY)
    // =========================

    public void SetGrounded(bool value) => isGrounded = value;

    public void SetWallLeft(bool value) => isTouchingWallLeft = value;
    public void SetWallRight(bool value) => isTouchingWallRight = value;

    public void SetHasJump(bool value) => hasJump = value;
    public void SetHasDash(bool value) => hasDash = value;

    public void SetGrab(bool value) => grab = value;
    public void SetCanGrab(bool value) => canGrab = value;

    public void SetDashing(bool value) => isDashing = value;
    public void SetWallJumping(bool value) => isWallJumping = value;

    public void SetCoyoteTimer(float value) => coyoteTimer = value;
    public void SetJumpBufferTimer(float value) => jumpBufferTimer = value;
}