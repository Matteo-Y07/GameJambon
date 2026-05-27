using UnityEngine;

public class DebuffProjectile : MonoBehaviour
{
    public enum Type
    {
        Ice, Slow, Pollution
    }

    public Type type;
    public float duration = 3f;
    public float slowMultiplier = 0.5f;

    private Rigidbody2D rb;
    [SerializeField] private float speed = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }

        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        switch (type)
        {
            case Type.Ice:
                player.ApplyIce(duration);
                break;

            case Type.Slow:
                player.ApplySlow(slowMultiplier, duration);
                break;

            case Type.Pollution:
                GarbageBar bar = player.GetGarbageBar();
                if (bar != null) bar.Add(10f);
                break;
        }

        Destroy(gameObject);
    }
}