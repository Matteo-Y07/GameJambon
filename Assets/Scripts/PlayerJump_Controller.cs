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
        if (!Input.GetButtonDown("Jump")) return;

        // Wall jump prioritaire
        if (!player.isGrounded && (player.isTouchingWallLeft || player.isTouchingWallRight))
        {
            player.StartCoroutine(WallJump());
            return;
        }

        // Saut normal
        if ((player.isGrounded || player.grab) && player.hasJump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.hasJump = false;
            player.grab = false;
        }
    }

    IEnumerator WallJump()
    {
        player.grab = false;
        player.isWallJumping = true; // bloque le controle horizontal dans PlayerMovement

        // mur a gauche donc vers la droite
        if (player.isTouchingWallLeft)
            player.rb.velocity = new Vector2(player.wallJumpImpulseX, (player.jumpForce/1.3f));

        // mur a droite donc vers la gauche
        else if (player.isTouchingWallRight)
            player.rb.velocity = new Vector2(-player.wallJumpImpulseX, (player.jumpForce/1.3f));

        yield return new WaitForSeconds(player.wallJumpImpulseTime);

        player.isWallJumping = false; //remet le contrôle horizontal
        player.hasJump = true;
    }
}