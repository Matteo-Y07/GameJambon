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
        if (garbageBar == null) Debug.LogError("GarbageBar not found in the scene.");
        if (playerHealth == null) Debug.LogError("PlayerHealth reference not set.");
        
    }

    void Update()
    {
        isDead = (garbageBar != null && garbageBar.IsMaxReached()) || (playerHealth != null && playerHealth.IsDead());
        if (isDead) Die();
    }

    void Die()
    {
        Debug.Log("Die() appelé");
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerRespawn != null) playerRespawn.TriggerRespawn();
    }

    public void ResetState()
    {
        Debug.Log("stauts du mouvement du joueur avant dead" + playerMovement.enabled);
        isDead = false;
        Debug.Log("stauts du mouvement du joueur " + playerMovement.enabled);
        if (playerMovement != null && !playerMovement.enabled) playerMovement.enabled = true;
    }
}