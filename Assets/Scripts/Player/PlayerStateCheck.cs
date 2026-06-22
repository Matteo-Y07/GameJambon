using UnityEngine;

public class PlayerStateCheck : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerRespawn playerRespawn;
    private GarbageBar garbageBar;
    private IntroFade introFade;
    private GameObject deathScreen;

    private bool deathHandled;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRespawn = GetComponent<PlayerRespawn>();

        garbageBar = FindObjectOfType<GarbageBar>(true);
        introFade = FindObjectOfType<IntroFade>(true);
        deathScreen = GameObject.Find("DeathScreen");
    }

    private void Update()
    {
        bool diedByHP = playerHealth != null && playerHealth.IsDead();
        bool diedByGarbage = garbageBar != null && garbageBar.IsMaxReached();

        if (!deathHandled && (diedByHP || diedByGarbage))
            Die(DeathType.Normal);

        // ✅ RESPawn input DOIT être actif après mort uniquement
        if (deathHandled && Input.GetButtonDown("Submit"))
        {
            playerRespawn?.TriggerRespawn();
        }
    }

    public void Die(DeathType type)
    {
        if (deathHandled) return;

        deathHandled = true;

        if (playerMovement != null)
            playerMovement.isFrozen = true;

        foreach (Monster m in FindObjectsByType<Monster>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            m.Freeze();

        LifeSystem.Instance.LoseLife();

        if (type == DeathType.Normal)
        {
            if (deathScreen != null && LifeSystem.Instance.HasLives())
                StartCoroutine(deathScreen.GetComponent<DeathScreen>().FadeIn());
        }
    }

    public void Kill(DeathType type)
    {
        Die(type);
    }

    public void ResetState()
    {
        deathHandled = false;

        if (introFade != null)
            StartCoroutine(introFade.Fade(Color.black, Color.clear, 2f));

        if (playerMovement != null)
            playerMovement.isFrozen = false;

        foreach (Monster m in FindObjectsByType<Monster>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            m.Unfreeze();
    }
}