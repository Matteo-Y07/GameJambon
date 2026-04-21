using UnityEngine;
using System.Collections;

public class PlayerJump_Controller
{
    private PlayerMovement player;

    public PlayerJump_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        HandleVariableJump();
        HandleJumpInput();
    }

    private void HandleVariableJump()
    {
        if (Input.GetButtonUp("Jump") && player.GetRigidbody().velocity.y > 0f)
        {
            Vector2 v = player.GetRigidbody().velocity;
            player.GetRigidbody().velocity = new Vector2(
                v.x,
                v.y * player.GetJumpMultiplier()
            );
        }
    }

    private void HandleJumpInput()
    {
        if (!Input.GetButtonDown("Jump"))
            return;

        // Wall jump priority
        if (!player.IsGrounded() &&
            (player.IsTouchingWallLeft() || player.IsTouchingWallRight()))
        {
            player.StartCoroutine(WallJump());
            return;
        }

        // Normal jump
        if (player.GetJumpBufferTimer() > 0f &&
            (player.GetCoyoteTimer() > 0f || player.IsGrabbing()) &&
            player.HasJump())
        {
            player.GetRigidbody().velocity =
                new Vector2(player.GetRigidbody().velocity.x, player.GetJumpForce());

            player.SetHasJump(false);
            player.SetCoyoteTimer(0f);
            player.SetJumpBufferTimer(0f);
            player.SetGrab(false);
        }
    }

    private IEnumerator WallJump()
    {
        player.SetGrab(false);
        player.SetWallJumping(true);

        Vector2 vel = player.GetRigidbody().velocity;

        if (player.IsTouchingWallLeft())
        {
            player.GetRigidbody().velocity = new Vector2(
                player.GetWallJumpImpulseX(),
                player.GetJumpForce() / 1.5f
            );
        }
        else if (player.IsTouchingWallRight())
        {
            player.GetRigidbody().velocity = new Vector2(
                -player.GetWallJumpImpulseX(),
                player.GetJumpForce() / 1.5f
            );
        }

        yield return new WaitForSeconds(player.GetWallJumpImpulseTime());

        player.SetWallJumping(false);
        player.SetHasJump(true);
    }
}