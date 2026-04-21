using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private DeathMenu deathMenu;
    [SerializeField] private GarbageBar garbageBar;
    
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
        Debug.Log("PLAYER DIED");
        if (isDead) return;

        isDead = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (deathMenu != null)
            deathMenu.ShowMenu();
    }
}