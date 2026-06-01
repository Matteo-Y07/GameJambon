using UnityEngine;

public class CatFollower : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    public float followSpeed = 6f;
    public float minDistance = 1.5f;
    public float teleportDistance = 8f;

    [Header("Offset")]
    public Vector2 offset = new Vector2(-1f, 0.5f);

    [Header("Raycast")]
    public LayerMask obstacleLayer;
    public float wallCheckDistance = 0.4f;
    public float groundCheckDistance = 0.4f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        if (distance > teleportDistance)
        {
            transform.position = player.position;
            return;
        }

        if (distance < minDistance)
            return;

        if (distance < minDistance * 2f)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isRunning", true);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < minDistance)
            return;

        Vector2 targetPosition = (Vector2)player.position + offset;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // 🔴 RAYCAST MUR DEVANT
        RaycastHit2D wallHit = Physics2D.Raycast(
            transform.position,
            Vector2.right * Mathf.Sign(direction.x),
            wallCheckDistance,
            obstacleLayer
        );

        Debug.DrawRay(
            transform.position,
            Vector2.right * Mathf.Sign(direction.x) * wallCheckDistance,
            Color.red
        );

        // 🟡 SI MUR → on essaye de monter légèrement (éviter blocage)
        if (wallHit.collider != null)
        {
            Vector2 upCheck = Physics2D.Raycast(
                transform.position,
                Vector2.up,
                groundCheckDistance,
                obstacleLayer
            ).point;

            // petite montée forcée pour contourner
            direction.y += 0.5f;
        }

        Vector2 newPos = rb.position + direction * followSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);

        spriteRenderer.flipX = direction.x < 0;
    }
}