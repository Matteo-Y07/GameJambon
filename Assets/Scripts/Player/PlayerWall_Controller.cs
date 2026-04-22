using UnityEngine;
using System.Collections;

public class PlayerWall_Controller
{
    private PlayerMovement player;

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

    private void HandleSlide()
    {
        if ((player.IsTouchingWallLeft() || player.IsTouchingWallRight()) &&
            !player.IsGrabbing() &&
            !player.IsGrounded() &&
            player.GetRigidbody().velocity.y < 0f)
        {
            Vector2 v = player.GetRigidbody().velocity;
            player.GetRigidbody().velocity = new Vector2(v.x, -1f);
        }
    }

    private void HandleGrabInput()
    {
        if (Input.GetButtonDown("Grab") &&
            (player.IsTouchingWallLeft() || player.IsTouchingWallRight()) &&
            !player.IsGrabbing() &&
            player.CanGrab())
        {
            player.StartCoroutine(Grab());
            player.StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Grab()
    {
        float timer = 0f;

        player.SetGrab(true);
        player.GetRigidbody().gravityScale = 0f;

        while (timer < player.GetMaxTimerGrab() && player.IsGrabbing())
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float climb = 0f;

            bool onWall = player.IsTouchingWallLeft() || player.IsTouchingWallRight();

            if (onWall)
            {
                if (vertical < 0 &&
                    (player.IsTouchingWallLeftBottom() || player.IsTouchingWallRightBottom()))
                {
                    climb = -player.GetClimbSpeed();
                }
                else if (vertical > 0 &&
                         (player.IsTouchingWallLeftTop() || player.IsTouchingWallRightTop()))
                {
                    climb = player.GetClimbSpeed();
                }
            }

            player.GetRigidbody().velocity = new Vector2(0f, climb);

            if (Input.GetButtonUp("Grab"))
                player.SetGrab(false);

            timer += Time.deltaTime;
            yield return null;
        }

        player.SetGrab(false);
        player.GetRigidbody().gravityScale = player.GetGravity();
    }

    private IEnumerator Cooldown()
    {
        player.SetCanGrab(false);
        yield return new WaitForSeconds(3f);
        player.SetCanGrab(true);
    }

    private void HandleStateReset()
    {
        if (player.IsGrabbing())
        {
            player.SetHasJump(true);
            player.SetHasDash(true);
        }
    }
}