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

        if (Input.GetButtonDown("Attack")) 
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
            if (monster != null) monster.TakeDamage(player.GetDamage());
        }
        Debug.Log("Attaque !" + " Ennemis touchés: " + hits.Length + " | Prochain attaque dans: " + nextAttackTime + " secondes");
        
    }

    bool IsEnemyInFront(Transform target)
    {
        Vector2 directionToTarget = (target.position - player.transform.position).normalized;

        // direction du joueur (basée sur flipX)
        Vector2 facingDirection = player.GetSpriteRenderer().flipX ? Vector2.left : Vector2.right;

        // arc de ~45° devant le joueur
        return Vector2.Dot(facingDirection, directionToTarget) > 0.707f;
    }
}