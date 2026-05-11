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
        if (Time.time < nextAttackTime) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
            nextAttackTime = Time.time + player.GetAttackCooldown();
        }
    }

    void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.GetAttackPoint().position, player.GetAttackRange(), player.GetEnemyLayer());

        foreach (Collider2D hit in hits)
        {
            if (!IsEnemyInFront(hit.transform)) continue;
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(player.GetDamage());
            }
        }

        Debug.Log("Attaque ! Prochaine attaque dans " + player.GetAttackCooldown() + " secondes.");
    }

    bool IsEnemyInFront(Transform target)
    {
        Vector2 directionToTarget = (target.position - player.transform.position).normalized;

        Vector2 facingDirection;
            if (player.GetSpriteRenderer().flipX) facingDirection = Vector2.left;
            else facingDirection = Vector2.right;

        return Vector2.Dot(facingDirection, directionToTarget) > 0.707f;
    }
}