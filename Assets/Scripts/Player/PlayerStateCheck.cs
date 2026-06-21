using UnityEngine;

public class PlayerStateCheck : MonoBehaviour
{
    // Références
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerRespawn playerRespawn;

    private GarbageBar garbageBar;
    private IntroFade introFade;
    private GameObject deathScreen;

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
        if (introFade == null)
        {
            introFade = FindObjectOfType<IntroFade>(true);
        }
        if (deathScreen == null)
        {
            deathScreen = GameObject.Find("DeathScreen");
        }
    }

    void Update()
    {
        isDead = (garbageBar != null && garbageBar.IsMaxReached()) || (playerHealth != null && playerHealth.IsDead());

        if (!deathHandled && isDead)
            Die();

        if (deathHandled && Input.GetButtonDown("Submit"))
            playerRespawn.TriggerRespawn();
    }

    public void Die()
    {
        if (deathHandled) return;

        deathHandled = true;

        playerMovement.isFrozen = true;

        // 🔥 LIFE SYSTEM
        LifeSystem.Instance.LoseLife();

        if (deathScreen != null && LifeSystem.Instance.HasLives())
            StartCoroutine(deathScreen.GetComponent<DeathScreen>().FadeIn());
    }

    public void ResetState()
    {
        isDead = false;
        deathHandled = false;
        if (introFade != null)
            StartCoroutine(introFade.Fade(Color.black, Color.clear, 2f));
        if (playerMovement != null)
            playerMovement.enabled = true;
        
    }
}