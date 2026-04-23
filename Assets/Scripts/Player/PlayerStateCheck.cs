using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GarbageBar garbageBar;
    [SerializeField] private PlayerRespawn playerRespawn;

    [Header("State")]
    [SerializeField] private int pollution = 0;
    [SerializeField] private int maxPollution = 100;

    private bool isDead = false;
    
    void Update()
    {
        if (isDead) return;

        pollution = (int)garbageBar.GetValue();

        if (playerHealth.IsDead() || pollution >= maxPollution)
        {
            Die();
        }
    }

    public void AddPollution(int amount)
    {
        pollution = Mathf.Clamp(pollution + amount, 0, maxPollution);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("PLAYER DIED");
        Debug.Log(playerRespawn != null ? "PlayerRespawn found" : "PlayerRespawn NOT found");
        // Désactive les contrôles
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Lance respawn rapide
        if (playerRespawn != null)
            playerRespawn.Die();
    }

    public void ResetState()
    {
        isDead = false;
        if (playerMovement != null) playerMovement.enabled = true;
        pollution = 0;

        if (garbageBar != null) garbageBar.Set(0f);
    }
}