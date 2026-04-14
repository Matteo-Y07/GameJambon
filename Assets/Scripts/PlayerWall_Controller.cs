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
        if (Input.GetButtonDown("Grab") && (player.isTouchingWallLeft || player.isTouchingWallRight) && !player.grab && player.canGrab)
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

        while (timer < player.maxTimerGrab && player.grab) // Tant que le timer n'est pas écoulé et que le grab n'est pas relâché
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float climb = 0f;
            bool onWall = player.isTouchingWallLeft || player.isTouchingWallRight;
            if (onWall){

                // On ne peut descendre que si le bas touche encore
                if (vertical < 0 && (player.isTouchingWallLeftBottom || player.isTouchingWallRightBottom))
                {
                    climb = -player.climbSpeed;
                }

                // On ne peut monter que si le haut touche encore
                else if (vertical > 0 && (player.isTouchingWallLeftTop || player.isTouchingWallRightTop))
                {
                    climb = player.climbSpeed;
                }
            }

            else climb = 0f;

            player.rb.velocity = new Vector2(0f, climb); // Permet de grimper ou descendre le long du mur

            if (Input.GetButtonUp("Grab"))
                player.grab = false;

            timer += Time.deltaTime;
            yield return null;
        }

        player.grab = false;
        player.rb.gravityScale = player.gravity; // Réactive la gravité après le grab
    }

    IEnumerator Cooldown()
    {
        //Met un cooldown pour le grab
        player.canGrab = false;
        yield return new WaitForSeconds(3f);
        player.canGrab = true;
    }

    void HandleStateReset()
    {
        if (player.grab) // Permet de réinitialiser le saut et le dash après un grab
        {
            player.hasJump = true;
            player.hasDash = true;
        }
    }
}