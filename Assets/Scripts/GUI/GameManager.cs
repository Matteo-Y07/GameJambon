using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;

    public string nextSpawnPoint;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCheckpoint(Vector3 pos)
    {
        lastCheckpointPosition = pos;
        hasCheckpoint = true;
    }

    public Vector3 GetCheckpoint(Vector3 defaultPos)
    {
        return hasCheckpoint ? lastCheckpointPosition : defaultPos;
    }
}