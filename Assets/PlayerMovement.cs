using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{

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


    // Dash variables
    public float dashPower = 20.0f;
    public Vector2 dashDirection = new Vector2();
    public bool isDashing = false;
    public bool hasDash = true;
    public float gravity = 2.5f;

    public SpriteRenderer PlayerSpriteRenderer;

    private void Awake()
    {
        // prends le SpriteRenderer du joueur pour pouvoir le tourner à gauche ou à droite
        PlayerSpriteRenderer = GetComponent<SpriteRenderer>();
        
    }
    
    void Update()
    {
        KeyboardInput();
        LimitFallSpeed();
        CheckGround();
        CheckWalls();
        if (canGrab && (isTouchingWallLeft || isTouchingWallRight) && !grab){
            rb.velocity = new Vector2(rb.velocity.x, -1f); // On ralentit la chute quand on touche un mur pour un effet de slide
        }
    }

    void KeyboardInput()
    {
        // Déplacement horizontal
        if (!isDashing && !grab)
        {
            if (Input.GetAxisRaw("Horizontal") > 0 && !isDashing) playerMoveRight();
            else if (Input.GetAxisRaw("Horizontal") < 0 && !isDashing) playerMoveLeft();
            else if (!isDashing) playerStop();
        }

        // Dash
        if (Input.GetButtonDown("Jump") && !isGrounded && hasDash) StartCoroutine(playerDash());

        // Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || grab) && hasJump)
        {
            playerJump();
        }

        //Grab Walls
        if (Input.GetKeyDown(KeyCode.LeftShift) && (isTouchingWallLeft || isTouchingWallRight) && !grab && canGrab)
        {
            Debug.Log("Grab");
            StartCoroutine(playerGrabWall());
            StartCoroutine(canGrabWall());
        }
        else if (!isDashing && !grab && !canGrab) rb.gravityScale = gravity; // On remet la gravité normale quand on arrête de grab
    }

    void playerMoveRight()
    {
        PlayerSpriteRenderer.flipX = false;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void playerMoveLeft()
    {
        PlayerSpriteRenderer.flipX = true;
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
    }

    void playerStop() {rb.velocity = new Vector2(0.0f, rb.velocity.y);}

    void playerJump(){
        if (hasJump) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasJump = false;
        }
    }

    IEnumerator playerDash()
    {
        isDashing = true;
        
        hasDash = false;
        Vector2 dashDirection = Vector2.zero;

        // dash dans la direction du mouvement
        if (Input.GetAxis("Horizontal") > 0) dashDirection.x += 1.0f;   // droite
        if (Input.GetAxis("Horizontal") < 0) dashDirection.x -= 1.0f;   // gauche
        if (Input.GetAxis("Vertical") > 0) dashDirection.y += 1.0f;   // haut
        if (Input.GetAxis("Vertical") < 0) dashDirection.y -= 1.0f;   // bas

        // Si aucune touche, dash vers le haut
        if (dashDirection == Vector2.zero)
            dashDirection = Vector2.up;

        dashDirection.Normalize(); // On normalise la direction pour que les diagonales ne soient pas plus rapides (pas comme dans Minecraft)
        rb.gravityScale = 0.0f; // On désactive la gravité pendant le dash
        rb.velocity = Vector2.zero; // On stop le mouvement du joueur avant de dash pour éviter les problèmes de momentum
        yield return new WaitForSeconds(0.03f); // pause légère juste avant de dash
        rb.velocity = dashDirection * dashPower; // On dash
        yield return new WaitForSeconds(0.05f); // Durée du dash
        rb.velocity = dashDirection * moveSpeed; // On remet la vitesse qu'on avait avant le dash
        rb.gravityScale = gravity; // On réactive la gravité
        isDashing = false;
    }

    IEnumerator playerGrabWall()
{
    grab = true;
    float timer = 0f; // timer max pour le grab
    rb.gravityScale = 0f; // On désactive la gravité pendant le grab

    while (timer < maxTimerGrab && grab)
    {
        // grab
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        timer += Time.deltaTime; // incrémente le timer
        if (Input.GetKeyUp(KeyCode.LeftShift)) grab = false;
        yield return null;
    }
    grab = false;
    rb.gravityScale = gravity;
    
}

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapArea(GroundCheckLeft.position, GroundCheckRight.position, groundLayer);
        if (isGrounded){
            hasJump = true; // Récupère le jump quand on touche le sol
            hasDash = true; // Récupère le dash quand on touche le sol
        }
    }

    void CheckWalls()
    {
        isTouchingWallLeft = Physics2D.OverlapArea(WallCheckLeft.position,WallCheckLeft.position + Vector3.right * 0.1f, wallLayer);
        isTouchingWallRight = Physics2D.OverlapArea(WallCheckRight.position,WallCheckRight.position + Vector3.left * 0.1f, wallLayer);
        if (grab){
            hasJump = true;
            hasDash = true;// Récupère le jump et le dash quand on touche un mur
        }
    }

    IEnumerator canGrabWall()
    {
        canGrab = false;
        yield return new WaitForSeconds(3f); // cooldown de 3 secondes
        canGrab = true;
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed && !isDashing){
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }
}