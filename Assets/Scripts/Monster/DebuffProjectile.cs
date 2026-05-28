using UnityEngine;

public class DebuffProjectile : MonoBehaviour
{
    public enum Type
    {
        Ice,
        Slow,
        Pollution
    }

    public Type type;

    public float duration = 3f;
    public float slowMultiplier = 0.5f;
    public float gravity = 0f;

    [SerializeField] private float speed = 8f;
    [SerializeField] private MonsterDebuff owner;

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (animator != null)
            animator.SetInteger("Type", (int)type);

    }

    // ✔ Tir STRICTEMENT horizontal
    public void Initialize(Vector2 direction)
    {
        Vector2 horizontalDir = direction.x >= 0 ? Vector2.right : Vector2.left;

        rb.velocity = horizontalDir * speed;
        rb.gravityScale = gravity;

        // Flip visuel
        Vector3 scale = transform.localScale;

        if (horizontalDir.x < 0)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Projectile de type " + type + " a touché le joueur !");

            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                switch (type)
                {
                    case Type.Ice:
                        if (animator != null)
                            animator.SetInteger("Type", 0);

                        player.ApplyIce(duration);
                        break;

                    case Type.Slow:
                        if (animator != null)
                            animator.SetInteger("Type", 1);

                        player.ApplySlow(slowMultiplier, duration);
                        break;

                    case Type.Pollution:
                        if (animator != null)
                            animator.SetInteger("Type", 2);

                        GarbageBar bar = player.GetGarbageBar();

                        if (bar != null)
                            bar.Add(10f);

                        break;
                }
            }
        }

        Destroy(gameObject);
    }
}