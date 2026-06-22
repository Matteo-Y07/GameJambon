using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeSystem : MonoBehaviour
{
    public static LifeSystem Instance;

    public string sceneToLoadOnGameOver = "MenuGameOver";

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
        currentLives = maxLives;
    }

    public int GetLives() => currentLives;

    public bool HasLives() => currentLives > 0;

    public void LoseLife()
    {
        currentLives--;

        if (currentLives <= 0)
            GameOver();
    }

    public void ResetLives()
    {
        currentLives = maxLives;
    }

    private void GameOver()
    {
        ResetLives();
        SceneManager.LoadScene(sceneToLoadOnGameOver);
    }
}