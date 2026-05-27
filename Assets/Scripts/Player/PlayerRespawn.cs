using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float respawnDelay = 0.2f;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStateCheck stateCheck;
    [SerializeField] private Rigidbody2D rb;

    private GarbageBar garbageBar;

    private Vector3 startPosition;
    private bool isRespawning;

    void Start()
    {
        startPosition = transform.position;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        garbageBar = FindObjectOfType<GarbageBar>();
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
        if (rb != null) rb.velocity = Vector2.zero;
        if (stateCheck != null) stateCheck.ResetState();

        isRespawning = false;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}