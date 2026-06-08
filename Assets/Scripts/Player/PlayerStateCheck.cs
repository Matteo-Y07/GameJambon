using UnityEngine;

public class PlayerStateCheck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerRespawn playerRespawn;

    private GarbageBar garbageBar;

    private bool isDead;
    private bool deathHandled;

    void Start()
    {
        garbageBar = FindObjectOfType<GarbageBar>();
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