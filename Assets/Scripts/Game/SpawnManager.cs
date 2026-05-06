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
            GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");

            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                GameManager.instance.SetCheckpoint(spawnPoint.transform.position);
            }
        }
    }
}