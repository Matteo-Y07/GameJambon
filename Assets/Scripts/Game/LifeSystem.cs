using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public static LifeSystem Instance;

    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentLives = maxLives;
    }

    public int GetLives() => currentLives;

    public bool HasLives() => currentLives > 0;

    public void LoseLife()
    {
        currentLives--;

        Debug.Log("nombre de vies restantes : " + currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void ResetLives()
    {
        currentLives = maxLives;
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        // ici tu peux load menu
        // SceneManager.LoadScene("GameOver");
    }
}