using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int health = 5;
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected int damage = 1;

    [Header("Detection")]
    [SerializeField] protected float detectionRange = 10f;
    [SerializeField] protected float attackRange = 1f;

    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected Transform player;
    protected bool dead = false;
    protected bool isFrozen = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    protected virtual void Update()
    {
        if (GameState.InDialogue || GameState.InPause)
            return;

        if (dead) return;

        if (isFrozen) return;

        Move();
    }

    protected virtual void Move()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectionRange) return;

        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

        spriteRenderer.flipX = direction.x < 0;
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        dead = true;
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void Freeze()
    {
        isFrozen = true;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0;
        rb.simulated = false;
    }

    public void Unfreeze()
    {
        isFrozen = false;

        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();

            if (pm != null)
                rb.gravityScale = pm.GetGravity();
        }

        rb.simulated = true;
    }

    //Setters & Getters
    
    public float GetDetectionRange() => detectionRange;
    public float GetAttackRange() => attackRange;
    public float GetMoveSpeed() => moveSpeed;
    public int GetDamage() => damage;
    public int GetHealth() => health;

    public void SetDetectionRange(float range) => detectionRange = range;
    public void SetAttackRange(float range) => attackRange = range;
    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    public void SetDamage(int dmg) => damage = dmg;
    public void SetHealth(int hp) => health = hp;


}