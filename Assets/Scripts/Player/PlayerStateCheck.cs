using UnityEngine;

public class PlayerStateCheck : MonoBehaviour
{
    // Références
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerRespawn playerRespawn;

    private GarbageBar garbageBar;

    private bool isDead;
    private bool deathHandled;

    void Awake()
    {
        // Trouve automatiquement les scripts frères sur le même GameObject
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (playerRespawn == null) playerRespawn = GetComponent<PlayerRespawn>();
    }

    void Start()
    {
        // Trouve automatiquement la GarbageBar dans le camion UI
        if (garbageBar == null)
        {
            garbageBar = FindObjectOfType<GarbageBar>(true);
        }
    }

    void Update()
    {
        if (deathHandled) return;
        isDead = (garbageBar != null && garbageBar.IsMaxReached()) || (playerHealth != null && playerHealth.IsDead());

        if (isDead)
            Die();
    }

    void Die()
    {
        deathHandled = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerRespawn != null)
            playerRespawn.TriggerRespawn();
    }

    public void ResetState()
    {
        isDead = false;
        deathHandled = false;

        if (playerMovement != null)
            playerMovement.enabled = true;
    }
}