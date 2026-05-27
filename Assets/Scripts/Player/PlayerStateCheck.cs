using UnityEngine;

public class PlayerStateCheck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerRespawn playerRespawn;

    private GarbageBar garbageBar;
    private bool isDead;
    void Start()
    {
        garbageBar = FindObjectOfType<GarbageBar>();
        
    }

    void Update()
    {
        isDead = (garbageBar != null && garbageBar.IsMaxReached()) || (playerHealth != null && playerHealth.IsDead());
        if (isDead) Die();
    }

    void Die()
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerRespawn != null) playerRespawn.TriggerRespawn();
    }

    public void ResetState()
    {
        isDead = false;
        if (playerMovement != null && !playerMovement.enabled) playerMovement.enabled = true;
    }
}