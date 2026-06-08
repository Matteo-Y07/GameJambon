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
    private bool hasTarget;

    private Vector3 firePointBasePos;

    protected override void Start()
    {
        base.Start();

        detectionRange = 6f;
        attackRange = 5f;

        SetDetectionRange(detectionRange);
        SetAttackRange(attackRange);
        SetMoveSpeed(1.5f);
        SetDamage(0);
        SetHealth(2);

        shootTimer = 0f;

        if (firePoint != null)
            firePointBasePos = firePoint.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        if (dead || player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        UpdateTargetState(distance);

        if (!hasTarget)
            return;

        MoveToPlayer(distance);
        HandleShoot(distance);
    }

    private void UpdateTargetState(float distance)
    {
        if (!hasTarget && distance <= detectionRange)
            hasTarget = true;

        if (hasTarget && distance > loseTargetRange)
        {
            hasTarget = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveToPlayer(float distance)
    {
        if (distance <= attackRange)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = player.position.x > firePoint.position.x ? Vector2.right : Vector2.left;
        rb.velocity = dir * moveSpeed;

        bool facingLeft = dir.x < 0;
        spriteRenderer.flipX = facingLeft;

        UpdateFirePoint(facingLeft);
    }

    private void UpdateFirePoint(bool facingLeft)
    {
        if (firePoint == null)
            return;

        firePoint.localPosition = new Vector3(facingLeft ? -Mathf.Abs(firePointBasePos.x) : Mathf.Abs(firePointBasePos.x), firePointBasePos.y, firePointBasePos.z);
    }

    private void HandleShoot(float distance)
    {
        if (distance > attackRange)
            return;

        shootTimer -= Time.deltaTime;

        if (shootTimer > 0f)
            return;

        Shoot();
        shootTimer = shootCooldown;
    }

    private void Shoot()
    {
        if (firePoint == null || projectilePrefabs == null || projectilePrefabs.Length == 0)
        {
            Debug.LogWarning("MonsterDebuff: missing firePoint or projectile prefabs");
            return;
        }

        int index = Random.Range(0, projectilePrefabs.Length);

        GameObject projectile = Instantiate(
            projectilePrefabs[index],
            firePoint.position,
            Quaternion.identity
        );

        DebuffProjectile debuff = projectile.GetComponent<DebuffProjectile>();

        if (debuff == null)
        {
            Debug.LogWarning("Projectile missing DebuffProjectile script");
            return;
        }

        Vector2 dir = (player.position - firePoint.position).normalized;
        debuff.Initialize(dir);
        Collider2D projectileCol = projectile.GetComponent<Collider2D>();
        Collider2D monsterCol = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(projectileCol, monsterCol);
    }
}