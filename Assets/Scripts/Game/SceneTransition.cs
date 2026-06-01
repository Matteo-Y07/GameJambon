using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            GameManager.instance.nextSpawnPoint = spawnPointName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}