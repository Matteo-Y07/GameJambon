using UnityEngine;
using System.Collections;

public class PlayerJump_Controller
{
    PlayerMovement player;

    public PlayerJump_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        // Saut variable : relâcher pour sauter moins haut
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C)) && player.rb.velocity.y > 0f)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * player.jumpMultiplier);
        }

        if (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.C)) return;

        // Wall jump prioritaire
        if (!player.isGrounded && (player.isTouchingWallLeft || player.isTouchingWallRight))
        {
            player.StartCoroutine(WallJump());
            return;
        }

        // Saut normal
        if (player.jumpBufferTimer > 0f && (player.coyoteTimer > 0f || player.grab) && player.hasJump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.hasJump = false;
            player.coyoteTimer = 0f;
            player.jumpBufferTimer = 0f;
            player.grab = false;
        }
    }

    IEnumerator WallJump()
    {
        player.grab = false;
        player.isWallJumping = true; // bloque le controle horizontal dans PlayerMovement

        // mur a gauche donc vers la droite
        if (player.isTouchingWallLeft)
            player.rb.velocity = new Vector2(player.wallJumpImpulseX, (player.jumpForce/1.5f));

        // mur a droite donc vers la gauche
        else if (player.isTouchingWallRight)
            player.rb.velocity = new Vector2(-player.wallJumpImpulseX, (player.jumpForce/1.5f));

        yield return new WaitForSeconds(player.wallJumpImpulseTime);

        player.isWallJumping = false; //remet le contrôle horizontal
        player.hasJump = true;
    }
}