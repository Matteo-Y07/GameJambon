using UnityEngine;
using System.Collections;

public class PlayerJumpController
{
    private PlayerMovement player;

    public PlayerJumpController(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        HandleJumpInput();
        HandleVariableJump();
        HandleJumpState();
    }

    private void HandleJumpInput()
    {
        if (!Input.GetButtonDown("Jump")) return;

        // Priorité wall jump
        if (!player.IsGrounded() &&
            (player.IsTouchingWallLeft() || player.IsTouchingWallRight()))
        {
            player.StartCoroutine(WallJump());
            return;
        }

        // NORMAL JUMP
        if (player.GetJumpBufferTimer() > 0f && (player.GetCoyoteTimer() > 0f || player.IsTouchingWallLeft() || player.IsTouchingWallRight()) && player.HasJump() && !player.IsGrabbing())
        {
            Rigidbody2D rb = player.GetRigidbody();

            player.SetJumping(true);
            rb.velocity = new Vector2(rb.velocity.x, player.GetJumpForce());

            player.SetHasJump(false);
            player.SetCoyoteTimer(0f);
            player.SetJumpBufferTimer(0f);
            player.SetGrab(false);
        }
    }

    private void HandleVariableJump()
    {
        if (Input.GetButtonUp("Jump") && player.GetRigidbody().velocity.y > 0f)
        {
            Rigidbody2D rb = player.GetRigidbody();
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * player.GetJumpMultiplier());
        }
    }

    private IEnumerator WallJump()
    {
        player.SetGrab(false);
        player.SetWallJumping(true);

        Rigidbody2D rb = player.GetRigidbody();
        float direction = player.IsTouchingWallLeft() ? 1f : -1f;
        rb.velocity = new Vector2(player.GetWallJumpImpulseX() * direction, player.GetJumpForce());
        player.SetJumping(true);

        yield return new WaitForSeconds(player.GetWallJumpImpulseTime());

        player.SetWallJumping(false);
        player.SetHasJump(true);
    }

    private void HandleJumpState()
    {
        Rigidbody2D rb = player.GetRigidbody();

        if (player.IsGrounded())
        {
            player.SetJumping(false);
            return;
        }

        if (rb.velocity.y > 0f)player.SetJumping(true);
        else player.SetJumping(false);
    }
}