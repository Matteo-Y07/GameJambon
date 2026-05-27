using UnityEngine;

public class MonsterDebuff : Monster
{
    [Header("Detection")]
    [SerializeField] private float loseTargetRange = 8f;

    [Header("Shoot")]
    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 2f;

    private float shootTimer;
    private bool hasTarget = false;
    
    protected override void Start()
    {
        base.Start();
        shootTimer = shootCooldown;
        SetDetectionRange(6f);
        SetAttackRange(5f);
        detectionRange = GetDetectionRange();
        attackRange = GetAttackRange();
        SetMoveSpeed(1.5f);
        SetDamage(0);
        SetHealth(2);
    }

    protected override void Update()
    {
        base.Update();

        if (dead || player == null)
            return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        if (!hasTarget && distance <= detectionRange)
            hasTarget = true;

        if (hasTarget && distance > loseTargetRange)
        {
            hasTarget = false;
            rb.velocity = Vector2.zero;
            return;
        }

        if (!hasTarget) return;

        MoveToPlayer(distance);
        HandleShoot(distance);
    }

    private void MoveToPlayer(float distance)
    {
        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        spriteRenderer.flipX = direction.x < 0;
    }

    private void HandleShoot(float distance)
    {
        if (distance > attackRange) return;

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        if (firePoint == null || projectilePrefabs.Length == 0) return;

        int index = Random.Range(0, projectilePrefabs.Length);

        GameObject projectile = Instantiate(projectilePrefabs[index], firePoint.position, Quaternion.identity);
        DebuffProjectile debuff = projectile.GetComponent<DebuffProjectile>();

        if (debuff == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        debuff.Initialize(dir);
    }
}