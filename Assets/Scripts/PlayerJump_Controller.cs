using UnityEngine;

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

        // Wall jump gauche (mur à gauche, on s'éjecte à droite)
        if (player.isTouchingWallLeft && !player.isGrounded)
        {
            player.grab = false;
            float horizontalBoost = player.moveSpeed * 1.5f;
            player.rb.velocity = new Vector2(horizontalBoost, player.jumpForce);
            player.hasJump = true;
            return;
        }

        // Wall jump droite (mur à droite, on s'éjecte à gauche)
        if (player.isTouchingWallRight && !player.isGrounded)
        {
            player.grab = false;
            float horizontalBoost = player.moveSpeed * 1.5f;
            player.rb.velocity = new Vector2(-horizontalBoost, player.jumpForce);
            player.hasJump = true;
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
}