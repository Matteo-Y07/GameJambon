using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDistance = 8f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMonster), 1f, spawnInterval);
    }

    void SpawnMonster()
    {
        if (player == null) return;

        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 spawnPos = (Vector2)player.position + randomDir * spawnDistance;

        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
    }
}