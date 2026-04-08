using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float maxFallSpeed = 8f;

    public Rigidbody2D rb;
    public Transform GroundCheckLeft;
    public Transform GroundCheckRight;
    public LayerMask groundLayer;
    public bool isGrounded= false;
    public bool hasJump = true;

    // Dash variables
    public float dashPower = 25f;
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
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    void KeyboardInput()
    {
        // Déplacement horizontal
        if (Input.GetAxisRaw("Horizontal") > 0 && !isDashing) playerMoveRight();

        else if (Input.GetAxisRaw("Horizontal") < 0 && !isDashing) playerMoveLeft();

        else if (!isDashing) playerStop();

        // Dash
        if (Input.GetButtonDown("Jump") && !isGrounded && !hasJump && hasDash) StartCoroutine(playerDash());

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            hasJump = true;
            playerJump();
        }
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

    void playerStop() {rb.velocity = new Vector2(0f, rb.velocity.y);}
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
        if (Input.GetAxisRaw("Horizontal") > 0) dashDirection.x += 1f;   // droite
        if (Input.GetAxisRaw("Horizontal") < 0) dashDirection.x -= 1f;   // gauche
        if (Input.GetAxisRaw("Vertical") > 0) dashDirection.y += 1f;   // haut
        if (Input.GetAxisRaw("Vertical") < 0) dashDirection.y -= 1f;   // bas

        // Si aucune touche, dash vers le haut
        if (dashDirection == Vector2.zero)
            dashDirection = Vector2.up;

        dashDirection.Normalize(); // On normalise la direction pour que les diagonales ne soient pas plus rapides (pas comme dans Minecraft)
        rb.gravityScale = 0f; // On désactive la gravité pendant le dash
        rb.velocity = dashDirection * dashPower; // On dash

        yield return new WaitForSeconds(0.1f); // Durée du dash
        rb.velocity = dashDirection * moveSpeed; // On remet la vitesse qu'on avait avant le dash
        rb.gravityScale = gravity; // On réactive la gravité
        isDashing = false;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapArea(GroundCheckLeft.position, GroundCheckRight.position, groundLayer);
        if (isGrounded){
            hasJump = false;
            hasDash = true; // Récupère le dash quand on touche le sol
        }
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed){
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
    }
}
