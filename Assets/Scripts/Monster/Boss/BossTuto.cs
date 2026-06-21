using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossTuto : MonoBehaviour
{

    [Header("IA")]
    public Transform player;
    public float tooCloseDistance = 3f;
    public float moveSpeed = 4f;
    public float wallCheckDistance = 3f;
    public float teleportDistance = 5f;

    private Rigidbody2D rb;

    [Header("Boss Stats")]
    public float maxHealth = 1000f;
    public float currentHealth;

    [Header("UI (Automated)")]
    [SerializeField] private GameObject healthBarUI;
    private Image healthBar;

    [Header("Activation")]
    public bool bossActivated = false;

    [Header("Spawn Monstres")]
    public GameObject[] monsterPrefabs; 
    public float spawnRadius = 4f;
    public float spawnInterval = 2f;
    public Transform groundCheck; 

    private SpriteRenderer spriteRenderer;
    private Coroutine spawnCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        healthBarUI = GameObject.Find("BossHealthBar");

        if (healthBarUI != null)
        {
            Transform fill = healthBarUI.transform.Find("Fill");

            if (fill != null)
            {
                healthBar = fill.GetComponent<Image>();
                healthBar.fillAmount = 1f;
            }

            healthBarUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!bossActivated || player == null)
            return;

        HandleSpriteDirection();
        HandleAI();
    }

    public void SetHealthBarUI(GameObject ui)
    {
        healthBarUI = ui;
    }

    public void ActivateBoss()
    {
        if (bossActivated) return;

        bossActivated = true;

        if (healthBarUI != null)
            healthBarUI.SetActive(true);

        spawnCoroutine = StartCoroutine(SpawnMonsters());
    }

    void HandleSpriteDirection()
    {
        if (player == null || spriteRenderer == null)
            return;

        if (player.position.x > transform.position.x)
            spriteRenderer.flipX = true; // regarde à droite
        else
            spriteRenderer.flipX = false;  // regarde à gauche
    }

    void HandleAI()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > tooCloseDistance)
            return;

        // Direction pour s'éloigner du joueur
        Vector2 fleeDirection = new Vector2(transform.position.x - player.position.x, transform.position.y).normalized;

        // Vérifie s'il y a un mur derrière le boss
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, fleeDirection, wallCheckDistance, LayerMask.GetMask("Ground"));

        if (wallHit.collider != null)
        {
            TeleportBehindPlayer();
            return;
        }

        transform.position += (Vector3)(fleeDirection * moveSpeed * Time.deltaTime);
    }

    void TeleportBehindPlayer()
    {
        Vector2 directionFromBossToPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y).normalized; 

        Vector2 teleportPos = new Vector2(player.position.x + directionFromBossToPlayer.x * teleportDistance, player.position.y);

        transform.position = teleportPos;
    }

    IEnumerator SpawnMonsters()
    {
        while (bossActivated)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefabs.Length == 0) return;

        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

        // CORRECTION 2D : En 2D, on utilise l'axe Y pour la hauteur, pas l'axe Z !
        Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        RaycastHit2D hit = Physics2D.Raycast(spawnPos + Vector3.up * 5f, Vector2.down, 50f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            spawnPos.y = hit.point.y + 0.5f; // Ajuste la position pour que le monstre apparaisse légèrement au-dessus du sol
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Boss touché : " + damage);

        if (!bossActivated) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.fillAmount = currentHealth / maxHealth;

        Debug.Log("PV restants : " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        bossActivated = false;

        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        // On masque la barre de vie à la mort du boss
        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        Destroy(gameObject);
    }
}