using UnityEngine;
using System.Collections;

public class PlayerDash_Controller
{
    PlayerMovement player;

    public PlayerDash_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        if (Input.GetButtonDown("Jump") && !player.isGrounded && player.hasDash
            && !player.grab
            && !player.isTouchingWallLeft
            && !player.isTouchingWallRight)
        {
            player.StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        player.isDashing = true;
        player.hasDash = false;

        Vector2 dir = Vector2.zero;

        if (Input.GetAxis("Horizontal") > 0) dir.x += 1;
        if (Input.GetAxis("Horizontal") < 0) dir.x -= 1;
        if (Input.GetAxis("Vertical") > 0) dir.y += 1;
        if (Input.GetAxis("Vertical") < 0) dir.y -= 1;

        if (dir == Vector2.zero)
            dir = Vector2.up;

        dir.Normalize();

        player.rb.gravityScale = 0;
        player.rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.03f);

        player.rb.velocity = dir * player.dashPower;

        yield return new WaitForSeconds(0.05f);

        player.rb.velocity = dir * player.moveSpeed;
        player.rb.gravityScale = player.gravity;
        player.isDashing = false;
    }
}