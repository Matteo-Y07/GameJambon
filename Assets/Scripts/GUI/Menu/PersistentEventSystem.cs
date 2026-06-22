using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemPersistant : MonoBehaviour
{
    private static EventSystemPersistant instance;
    private EventSystem eventSystem;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        eventSystem = GetComponent<EventSystem>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventSystem[] systems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        foreach (var sys in systems)
        {
            if (sys != eventSystem)
            {
                Destroy(sys.gameObject);
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}