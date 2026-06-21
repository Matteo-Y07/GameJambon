using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject normalMonster;
    [SerializeField] private GameObject debuffMonster;
    [SerializeField] private GameObject pollutionMonster;
    [SerializeField] private GameObject sleepMonster;

    [Header("Spawn Points")]
    [SerializeField] private MonsterSpawnPoint[] spawnPoints;

    private bool hasSpawned = false;

    void Start()
    {
        SpawnAll();
    }

    void SpawnAll()
    {
        if (hasSpawned) return;

        hasSpawned = true;

        foreach (MonsterSpawnPoint point in spawnPoints)
        {
            if (point == null) continue;

            GameObject prefab = GetPrefab(point.GetSpawnType());

            if (prefab != null)
            {
                Instantiate(prefab, point.transform.position, Quaternion.identity);
            }

            Destroy(point.gameObject);
        }
    }

    GameObject GetPrefab(SpawnType type)
    {
        switch (type)
        {
            case SpawnType.Normal:
                return normalMonster;

            case SpawnType.Debuff:
                return debuffMonster;

            case SpawnType.Pollution:
                return pollutionMonster;

            case SpawnType.Sleep:
                return sleepMonster;

            default:
                return null;
        }
    }
}