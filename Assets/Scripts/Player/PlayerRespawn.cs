using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float respawnDelay = 0.2f;

    // Références
    private PlayerHealth playerHealth;
    private PlayerStateCheck stateCheck;
    private Rigidbody2D rb;

    private GarbageBar garbageBar;

    private Vector3 startPosition;
    private bool isRespawning;

    void Awake()
    {
        // Liens automatiques sur le même GameObject
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (stateCheck == null) stateCheck = GetComponent<PlayerStateCheck>();
    }

    void Start()
    {
        startPosition = transform.position;

        // Trouve automatiquement la GarbageBar dans le camion UI
        if (garbageBar == null)
        {
            garbageBar = FindObjectOfType<GarbageBar>(true);
        }
    }

    public void TriggerRespawn()
    {
        if (isRespawning) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        yield return new WaitForSeconds(respawnDelay);

        Vector3 respawnPos = GameManager.instance.GetCheckpoint(startPosition);
        transform.position = respawnPos;

        if (playerHealth != null) playerHealth.ResetHealth();
        if (garbageBar != null) garbageBar.ResetBar();

        rb.velocity = Vector2.zero;

        if (stateCheck != null)
            stateCheck.ResetState();

        playerHealth.ResetHealth();

        isRespawning = false;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}