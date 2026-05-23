// PlayerAnimation.cs
using UnityEngine;

public class PlayerAnimation
{
    private PlayerMovement player;

    public PlayerAnimation(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        
        // pour la course
        player.GetAnimator().SetBool("isGrounded", player.IsGrounded());
        player.GetAnimator().SetBool("isRunning", player.IsGrounded() && Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f);
        //pour le saut
        player.GetAnimator().SetBool("isGrounded", player.IsGrounded() && !player.IsJumping());

        //pour le grab
        player.GetAnimator().SetBool("isGrabbing", (player.IsTouchingWallLeft() || player.IsTouchingWallRight()) && player.IsGrabbing());

        //pour le dash
        //player.GetAnimator().SetBool("isDashing", player.IsDashing());

        //pour le slide
        player.GetAnimator().SetBool("isSliding", (player.IsTouchingWallLeft() || player.IsTouchingWallRight()) && !player.IsGrabbing() && !player.IsGrounded());

        //pour la chute
        player.GetAnimator().SetBool("isFalling", player.GetRigidbody().velocity.y < -0.1f && !player.IsGrounded() && !(player.IsTouchingWallLeft() || player.IsTouchingWallRight()));

        //pour l'atterissage
        player.GetAnimator().SetBool("isLanding", player.IsGrounded() && player.GetRigidbody().velocity.y < -0.1f);

        //pour l'escalade
        player.GetAnimator().SetBool("isClimbing", player.IsGrabbing() && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f);
    }
}