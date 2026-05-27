using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] private SpawnType spawnType;

    public SpawnType GetSpawnType() => spawnType;
}