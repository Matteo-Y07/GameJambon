using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (GameManager.instance == null || player == null) return;

        string spawnName = GameManager.instance.nextSpawnPoint;

        if (!string.IsNullOrEmpty(spawnName))
        {
            GameObject spawn = GameObject.Find(spawnName);

            if (spawn != null)
            {
                player.transform.position = spawn.transform.position;
            }
        }
    }
}