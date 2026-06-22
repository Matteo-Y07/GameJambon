using UnityEngine;

public class PersistentRoot : MonoBehaviour
{
    public static PersistentRoot Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}