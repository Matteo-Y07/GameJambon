using UnityEngine;

public class MonsterSleep : Monster
{
    [Header("Sleep")]
    [SerializeField] private float sleepDuration = 2f;

    private PlayerMovement playerMovement;

    private bool isSleeping = false;
    private float timer = 0f;

    private Collider2D monsterCollider;

    protected override void Awake()
    {
        base.Awake();

        monsterCollider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();
        if (isSleeping)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                playerMovement.enabled = true;

                Destroy(gameObject);
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (isSleeping)
            return;

        playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;

            timer = sleepDuration;
            isSleeping = true;

            // rend le monstre invisible
            spriteRenderer.enabled = false;

            // désactive les collisions
            monsterCollider.enabled = false;

            // stop le mouvement
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }
    }
}