using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 0.2f;

    private PlayerHealth playerHealth;
    private PlayerStateCheck stateCheck;
    private PlayerMovement player;
    private DeathScreen deathScreen;
    private Rigidbody2D rb;
    private GarbageBar garbageBar;

    private Vector3 startPosition;
    private bool isRespawning;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        stateCheck = GetComponent<PlayerStateCheck>();
        player = GetComponent<PlayerMovement>();

        deathScreen = FindObjectOfType<DeathScreen>(true);
    }

    void Start()
    {
        startPosition = transform.position;
        garbageBar = FindObjectOfType<GarbageBar>(true);
    }

    public void TriggerRespawn()
    {
        if (!isRespawning)
            StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = GameManager.instance.GetCheckpoint(startPosition);

        rb.velocity = Vector2.zero;

        playerHealth?.ResetHealth();
        garbageBar?.ResetBar();

        stateCheck?.ResetState();

        if (player != null)
            player.Unfreeze();

        foreach (Monster m in FindObjectsByType<Monster>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            m.Unfreeze();

        if (deathScreen != null)
            StartCoroutine(deathScreen.FadeOut());

        isRespawning = false;
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}