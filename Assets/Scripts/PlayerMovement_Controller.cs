using UnityEngine;

public class PlayerMovement_Controller
{
    PlayerMovement player;

    public PlayerMovement_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        if (player.isDashing || player.grab) return;

        float input = Input.GetAxisRaw("Horizontal");

        if (input < 0 && player.isTouchingWallLeft) {
            input = 0;
            player.PlayerSpriteRenderer.flipX = true;
        }

        if (input > 0 && player.isTouchingWallRight) {
            input = 0;
            player.PlayerSpriteRenderer.flipX = false;
        }

        if (input > 0) MoveRight();
        else if (input < 0) MoveLeft();
        else Stop();
    }

    void MoveRight()
    {
        player.PlayerSpriteRenderer.flipX = false;
        player.rb.velocity = new Vector2(player.moveSpeed, player.rb.velocity.y);
    }

    void MoveLeft()
    {
        player.PlayerSpriteRenderer.flipX = true;
        player.rb.velocity = new Vector2(-player.moveSpeed, player.rb.velocity.y);
    }

    void Stop()
    {
        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
    }
}