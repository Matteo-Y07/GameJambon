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

    void Start()
    {
        SpawnAll();
    }

    void SpawnAll()
    {
        foreach (MonsterSpawnPoint point in spawnPoints)
        {
            GameObject prefab = GetPrefab(point.GetSpawnType());

            if (prefab != null)
            {
                Instantiate(prefab, point.transform.position, Quaternion.identity);
            }
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