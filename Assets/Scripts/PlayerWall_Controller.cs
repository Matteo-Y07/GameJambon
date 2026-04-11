using UnityEngine;
using System.Collections;

public class PlayerWall_Controller
{
    PlayerMovement player;

    public PlayerWall_Controller(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        HandleSlide();
        HandleGrabInput();
        HandleStateReset();
    }

    void HandleSlide()
    {
        if ((player.isTouchingWallLeft || player.isTouchingWallRight) && !player.grab && !player.isGrounded && player.rb.velocity.y < 0)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, -1f);
        }
    }

    void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            (player.isTouchingWallLeft || player.isTouchingWallRight) &&
            !player.grab && player.canGrab)
        {
            player.StartCoroutine(Grab());
            player.StartCoroutine(Cooldown());
        }
    }

    IEnumerator Grab()
    {
        player.grab = true;
        float timer = 0f;

        player.rb.gravityScale = 0f;

        while (timer < player.maxTimerGrab && player.grab)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);

            if (Input.GetKeyUp(KeyCode.LeftShift))
                player.grab = false;

            timer += Time.deltaTime;
            yield return null;
        }

        player.grab = false;
        player.rb.gravityScale = player.gravity;
    }

    IEnumerator Cooldown()
    {
        player.canGrab = false;
        yield return new WaitForSeconds(3f);
        player.canGrab = true;
    }

    void HandleStateReset()
    {
        if (player.grab)
        {
            player.hasJump = true;
            player.hasDash = true;
        }
    }
}