using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossTuto : MonoBehaviour
{
    [Header("Boss Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI (Automated)")]
    private Slider healthBar;
    private GameObject healthBarUI;

    [Header("Activation")]
    public bool bossActivated = false;

    [Header("Spawn Monstres")]
    public GameObject[] monsterPrefabs; 
    public float spawnRadius = 4f;
    public float spawnInterval = 2f;
    public Transform groundCheck; 

    private Coroutine spawnCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

        // AUTOMATISATION : On trouve l'UI de la barre de boss dans le Canvas persistant
        // Remplace "BossHealthBar" par le nom EXACT de l'objet parent de ta barre de boss dans l'UI
        healthBarUI = GameObject.Find("BossHealthBar");

        if (healthBarUI != null)
        {
            // On récupère le Slider qui est sur cet objet ou dans ses enfants
            healthBar = healthBarUI.GetComponentInChildren<Slider>();
            
            // On configure le slider par rapport aux HP max du boss
            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;
            }

            // On cache la barre en attendant le combat
            healthBarUI.SetActive(false);
        }
        else
        {
            Debug.LogError("⚠️ BossTuto : Impossible de trouver l'objet 'BossHealthBar' dans le Canvas !");
        }
    }

    public void ActivateBoss()
    {
        if (bossActivated) return;

        bossActivated = true;

        if (healthBarUI != null)
            healthBarUI.SetActive(true);

        spawnCoroutine = StartCoroutine(SpawnMonsters());
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

        RaycastHit2D hit = Physics2D.Raycast(spawnPos + Vector3.up * 10f, Vector2.down, 50f);
        if (hit.collider != null)
        {
            spawnPos.y = hit.point.y;
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        if (!bossActivated) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
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