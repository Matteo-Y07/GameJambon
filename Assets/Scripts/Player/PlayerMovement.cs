using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
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

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackCooldown = 0.5f;

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

    [SerializeField] private Transform attackPoint;

    [SerializeField] private Animator animator;

    [Header("Runtime State")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
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

    private PlayerMovementController movement;
    private PlayerJumpController jump;
    private PlayerDashController dash;
    private PlayerWallController wall;
    private PlayerAttackController attack;
    private PlayerAnimation playerAnimation;
    
    private void Awake() 
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        movement = new PlayerMovementController(this);
        jump = new PlayerJumpController(this);
        dash = new PlayerDashController(this);
        wall = new PlayerWallController(this);
        attack = new PlayerAttackController(this);
        playerAnimation = new PlayerAnimation(this);
        playerCamera = GetComponent<Camera>();
        dashTrail = GetComponent<TrailRenderer>();
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Ground");
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
        attack.Handle();
        playerAnimation.Handle();
        ApplyFallGravity();
        LimitFallSpeed();
        
        if (IsGrounded() && GetRigidbody().velocity.y <= 0f) SetJumping(false);
    }

    void HandleCoyoteTime()
    {
        if (IsGrounded()) coyoteTimer = GetCoyoteTime();
        else coyoteTimer -= Time.deltaTime;
    }

    void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump")) jumpBufferTimer = GetJumpBufferTime();
        else jumpBufferTimer -= Time.deltaTime;
    }

    void CheckGround()
    {
        bool grounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, groundLayer);
        SetGrounded(grounded);
        if (grounded)
        {
            SetHasJump(true);
            SetHasDash(true);
        }
    }

    void CheckWalls()
    {
        bool leftTop = Physics2D.OverlapArea(wallCheckLeftTop.position, wallCheckLeftTop.position + Vector3.right * 0.1f, wallLayer);
        bool leftBottom = Physics2D.OverlapArea(wallCheckLeftBottom.position, wallCheckLeftBottom.position + Vector3.right * 0.1f, wallLayer);
        bool rightTop = Physics2D.OverlapArea(wallCheckRightTop.position, wallCheckRightTop.position + Vector3.left * 0.1f, wallLayer);
        bool rightBottom = Physics2D.OverlapArea(wallCheckRightBottom.position, wallCheckRightBottom.position + Vector3.left * 0.1f, wallLayer);

        SetWallLeft(leftTop || leftBottom);
        SetWallRight(rightTop || rightBottom);

        if (IsTouchingWallLeft() && spriteRenderer.flipX == false && !IsGrounded()) spriteRenderer.flipX = true;

        else if (IsTouchingWallRight() && spriteRenderer.flipX == true && !IsGrounded()) spriteRenderer.flipX = false;
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
        if (IsGrabbing() || IsDashing()) return;

        if (rb.velocity.y < 0f) rb.gravityScale = GetGravity() * 1.8f;
        else if (rb.velocity.y > 0f && !Input.GetButton("Jump")) rb.gravityScale = GetGravity() * 1.3f;
        else rb.gravityScale = GetGravity();
    }

    // =========================
    // GETTERS (CONFIG)
    // =========================

    //GETTERS PHISICS 

    public Rigidbody2D GetRigidbody() => rb;
    public float GetMoveSpeed() => moveSpeed;
    public float GetMaxFallSpeed() => maxFallSpeed;
    public float GetGravity() => gravity;


    //GETTERS JUMP MECHANICS 
    public float GetJumpForce() => jumpForce;
    public float GetJumpMultiplier() => jumpMultiplier;
    public float GetCoyoteTime() => coyoteTime;
    public float GetJumpBufferTime() => jumpBufferTime;


    //GETTERS WALL MECHANICS

    public float GetClimbSpeed() => climbSpeed;
    public float GetMaxTimerGrab() => maxTimerGrab;
    public float GetWallJumpImpulseTime() => wallJumpImpulseTime;
    public float GetWallJumpImpulseX() => wallJumpImpulseX;


    //GETTERS DASH MECHANICS

    public float GetDashPower() => dashPower;


    //GETTERS ATTACK MECHANICS

    public float GetAttackRange() => attackRange;
    public int GetDamage() => damage;
    public LayerMask GetEnemyLayer() => enemyLayer;
    public float GetAttackCooldown() => attackCooldown;
    public Transform GetAttackPoint() => attackPoint;


    //GETTERS VISUALS 

    public SpriteRenderer GetSpriteRenderer() => spriteRenderer;
    public TrailRenderer GetTrail() => dashTrail;


    //GETTERS CAMERA

    public Camera GetPlayerCamera() => playerCamera;


    // GETTERS ANIMATOR

    public Animator GetAnimator() => animator;




    // =========================
    // GETTERS (STATE)
    // =========================

    // PLAYER STATE

    public bool IsGrounded() => isGrounded;
    public bool IsJumping() => isJumping;
    public bool IsDashing() => isDashing;
    public bool IsWallJumping() => isWallJumping;
    public bool IsGrabbing() => grab;
    public bool CanGrab() => canGrab;


    // ABILITIES

    public bool HasJump() => hasJump;
    public bool HasDash() => hasDash;


    // TIMERS

    public float GetCoyoteTimer() => coyoteTimer;
    public float GetJumpBufferTimer() => jumpBufferTimer;


    // =========================
    // GETTERS (WALL DETECTION)
    // =========================

    // BASIC WALL CHECKERS

    public bool IsTouchingWallLeft() => isTouchingWallLeft;
    public bool IsTouchingWallRight() => isTouchingWallRight;


    // CORNER CHECKS

    public bool IsTouchingWallLeftTop()
    {
        return Physics2D.OverlapArea(wallCheckLeftTop.position, wallCheckLeftTop.position, wallLayer);
    }

    public bool IsTouchingWallLeftBottom()
    {
        return Physics2D.OverlapArea(wallCheckLeftBottom.position, wallCheckLeftBottom.position + Vector3.right * 0.1f, wallLayer);
    }

    public bool IsTouchingWallRightTop()
    {
        return Physics2D.OverlapArea(wallCheckRightTop.position, wallCheckRightTop.position, wallLayer);
    }

    public bool IsTouchingWallRightBottom()
    {
        return Physics2D.OverlapArea(wallCheckRightBottom.position, wallCheckRightBottom.position + Vector3.left * 0.1f, wallLayer);
    }

    // =========================
    // SETTERS (CONFIG)
    // =========================

    // MOVEMENT
    
    public void SetMoveSpeed(float value) => moveSpeed = value;


    // =========================
    // SETTERS (STATE)
    // =========================

    // PLAYER STATE

    public void SetGrounded(bool value) => isGrounded = value;
    public void SetJumping(bool value) => isJumping = value;
    public void SetDashing(bool value) => isDashing = value;
    public void SetWallJumping(bool value) => isWallJumping = value;
    public void SetGrab(bool value) => grab = value;
    public void SetCanGrab(bool value) => canGrab = value;


    // WALL STATE

    public void SetWallLeft(bool value) => isTouchingWallLeft = value;
    public void SetWallRight(bool value) => isTouchingWallRight = value;


    // ABILITIES

    public void SetHasJump(bool value) => hasJump = value;
    public void SetHasDash(bool value) => hasDash = value;


    // TIMERS

    public void SetCoyoteTimer(float value) => coyoteTimer = value;
    public void SetJumpBufferTimer(float value) => jumpBufferTimer = value;

}