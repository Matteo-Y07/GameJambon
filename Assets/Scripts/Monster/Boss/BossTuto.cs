using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossTuto : MonoBehaviour
{
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

    private Coroutine spawnCoroutine;

    void Start()
    {
        currentHealth = maxHealth;

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