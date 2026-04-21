using UnityEngine;
using System.Collections;

public class PlayerDash_Controller
{
    private PlayerMovement player;

    public PlayerDash_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        if (Input.GetButtonDown("Dash") &&
            !player.IsGrounded() &&
            player.HasDash() &&
            !player.IsGrabbing() &&
            !player.IsTouchingWallLeft() &&
            !player.IsTouchingWallRight())
        {
            player.StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        player.SetDashing(true);
        player.SetHasDash(false);

        Vector2 dir = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") > 0) dir.x = 1;
        else if (Input.GetAxisRaw("Horizontal") < 0) dir.x = -1;

        if (Input.GetAxisRaw("Vertical") > 0) dir.y = 1;
        else if (Input.GetAxisRaw("Vertical") < 0) dir.y = -1;

        if (dir == Vector2.zero)
            dir = Vector2.up;

        dir.Normalize();

<<<<<<< HEAD
        player.rb.gravityScale = 0;
        player.rb.velocity = Vector2.zero;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.04f);
        Time.timeScale = 1f;
=======
        Rigidbody2D rb = player.GetRigidbody();

        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;

>>>>>>> 5c2b33d0189b2f267db8fed62c1b8b3bf07d5132


        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.04f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.03f);

        rb.velocity = dir * player.GetDashPower();

        yield return new WaitForSeconds(0.1f);

        rb.velocity = dir * player.GetMoveSpeed();
        rb.gravityScale = player.GetGravity();

        player.SetDashing(false);
    }
}