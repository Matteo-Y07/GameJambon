using UnityEngine;
using System.Collections;
public class dashUpdate : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    // Dash variables
    public float dashPower = 10f;
    public Vector2 dashDirection = new Vector2();
    bool isDashing;
    private bool hasDash;

    public float gravityScale = 1.5f;
    public float maxFallSpeed = 8f;

    public Rigidbody2D rb;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public LayerMask groundLayer;

    private bool isGrounded;
    

    private SpriteRenderer PlayerSpriteRenderer;

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
        if (Input.GetKey("d") && !isDashing) playerMoveRight();

        else if (Input.GetKey("q") && !isDashing) playerMoveLeft();

        else if (!isDashing) playerStop();

        // Dash
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && hasDash) StartCoroutine(playerDash());

        // Jump
        if (Input.GetKey(KeyCode.Space) && isGrounded) playerJump();
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
    void playerJump(){rb.velocity = new Vector2(rb.velocity.x, jumpForce);}

    IEnumerator playerDash()
    {
        isDashing = true;
        hasDash = false;
        Vector2 dashDirection = Vector2.zero;

        // dash dans la direction du mouvement
        if (Input.GetKey("d")) dashDirection.x += 1f;   // droite
        if (Input.GetKey("q")) dashDirection.x -= 1f;   // gauche
        if (Input.GetKey("z")) dashDirection.y += 1f;   // haut
        if (Input.GetKey("s")) dashDirection.y -= 1f;   // bas

        // Si aucune touche, dash vers le haut
        if (dashDirection == Vector2.zero)
            dashDirection = new Vector2(0f, 1f);

        dashDirection.Normalize(); // On normalise la direction pour que les diagonales ne soient pas plus rapides (pas comme dans Minecraft)
        rb.velocity = dashDirection * dashPower; // On dash
        rb.gravityScale = 0f; // On désactive la gravité pendant le dash

        yield return new WaitForSeconds(0.1f); // Durée du dash
        rb.velocity = dashDirection * moveSpeed; // On remet la vitesse qu'on avait avant le dash
        isDashing = false;
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, LayerMask.GetMask("Ground"));
        if (isGrounded)
            hasDash = true; // Récupère le dash quand on touche le sol
    }

    void LimitFallSpeed()
    {
        if (rb.velocity.y < -maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
    }
}