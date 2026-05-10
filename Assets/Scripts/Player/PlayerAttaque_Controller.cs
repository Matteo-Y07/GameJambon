using UnityEngine;

public class PlayerAttackController
{
    private PlayerMovement player;
    private float nextAttackTime = 0f;

    public PlayerAttackController(PlayerMovement player)
    {
        this.player = player;
    }

    public void Handle()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + player.GetAttackCooldown();
            }
        }
    }

    void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            player.GetAttackPoint().position,
            player.GetAttackRange(),
            player.GetEnemyLayer()
        );

        foreach (Collider2D enemy in enemies)
        {
            if (!EnnemiEstDevant(enemy.transform)) continue;

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();

            if (health != null) health.TakeDamage(player.GetDamage());
        }

        Debug.Log("Attaque ! Prochain attaque dans " + player.GetAttackCooldown() + " secondes.");
    }

    bool EnnemiEstDevant(Transform enemy)
    {
        Vector2 directionToEnemy = (enemy.position - player.transform.position).normalized;

        Vector2 facingDirection;
        if (player.GetSpriteRenderer().flipX)
            facingDirection = Vector2.left;
        else
            facingDirection = Vector2.right;

        return Vector2.Dot(facingDirection, directionToEnemy) > 0.707f;
    }
}