using UnityEngine;

public class PlayerMovement_Controller
{
    private PlayerMovement player;

    public PlayerMovement_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        Debug.Log("Movement Controller active");
        Debug.Log(
            "Dash=" + player.IsDashing() +
            " Grab=" + player.IsGrabbing() +
            " WallJump=" + player.IsWallJumping()
        );
        if (player.IsDashing() || player.IsGrabbing() || player.IsWallJumping())
            return;

        float input = Input.GetAxisRaw("Horizontal");

        if (input < 0 && player.IsTouchingWallLeft())
            input = 0;

        if (input > 0 && player.IsTouchingWallRight())
            input = 0;

        if (input > 0)
            MoveRight();
        else if (input < 0)
            MoveLeft();
        else
            Stop();
    }

    private void MoveRight()
    {
        player.GetSpriteRenderer().flipX = false;

        player.GetRigidbody().velocity = new Vector2(
            player.GetMoveSpeed(),
            player.GetRigidbody().velocity.y
        );
    }

    private void MoveLeft()
    {
        player.GetSpriteRenderer().flipX = true;

        player.GetRigidbody().velocity = new Vector2(
            -player.GetMoveSpeed(),
            player.GetRigidbody().velocity.y
        );
    }

    private void Stop()
    {
        player.GetRigidbody().velocity = new Vector2(
            0f,
            player.GetRigidbody().velocity.y
        );
    }
}