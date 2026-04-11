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

        if ((player.isGrounded || player.grab || player.canGrab) && player.hasJump)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
            player.hasJump = false;
        }

        if (player.isTouchingWallLeft)
        {
            player.rb.velocity = new Vector2(player.moveSpeed, player.jumpForce);
            player.hasJump = true;
        }

        if (player.isTouchingWallRight)
        {
            player.rb.velocity = new Vector2(-player.moveSpeed, player.jumpForce);
            player.hasJump = true;
        }
    }
}