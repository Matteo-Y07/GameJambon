using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    private IntroFade introFade;
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Transition(string sceneToLoad, string spawnPointName)
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionRoutine(sceneToLoad, spawnPointName));
    }

    private IEnumerator TransitionRoutine(string sceneToLoad, string spawnPointName)
    {
        isTransitioning = true;

        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        IntroFade fade = FindFirstObjectByType<IntroFade>();
        player.Freeze();
        if (fade != null)
            yield return fade.Fade(Color.clear, Color.black, 1f);
        player.Unfreeze();
        GameManager.instance.nextSpawnPoint = spawnPointName;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;

        while (!op.isDone)
            yield return null;

        yield return null; // stabilisation

        player = FindFirstObjectByType<PlayerMovement>();
        fade = FindFirstObjectByType<IntroFade>();

        // 🧊 FREEZE 2D PROPRE
        if (player != null)
        {
            player.Freeze();
        }

        // FADE IN
        if (fade != null)
            yield return fade.Fade(Color.black, Color.clear, 1f);

        // 🔓 UNFREEZE
        if (player != null)
        {
            player.Unfreeze();
        }

        isTransitioning = false;
    }
}