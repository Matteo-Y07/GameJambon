using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string spawnPointName;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (!player.CompareTag("Player")) return;

        SceneTransitionManager.Instance.Transition(sceneToLoad, spawnPointName);
    }
}