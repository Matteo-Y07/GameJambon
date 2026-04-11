using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // === TOUT TON CODE (variables inchangées) ===

    public float moveSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public float maxFallSpeed = 8.0f;
    public float maxTimerGrab = 1f;

    public Rigidbody2D rb;
    public Transform GroundCheckLeft;
    public Transform GroundCheckRight;
    public Transform WallCheckLeft;
    public Transform WallCheckRight;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public Camera PlayerCamera;

    public bool isGrounded= false;
    public bool isTouchingWallLeft;
    public bool isTouchingWallRight;
    public bool hasJump = true;
    public bool grab = false;
    public bool canGrab = true;

    public float dashPower = 20.0f;
    public Vector2 dashDirection = new Vector2();
    public bool isDashing = false;
    public bool hasDash = true;
    public float gravity = 2.5f;

    public SpriteRenderer PlayerSpriteRenderer;

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
        movement.Handle();
        jump.Handle();
        dash.Handle();
        wall.Handle();

        LimitFallSpeed();
        CheckGround();
        CheckWalls();
    }

    // === TES FONCTIONS NON MODIFIÉES ===

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
        if (rb.velocity.y < -maxFallSpeed && !isDashing){
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }
}