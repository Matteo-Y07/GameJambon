using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject deathMenuUI;

    void Start()
    {
        if (deathMenuUI != null)
            deathMenuUI.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ShowMenu()
    {
        if (deathMenuUI == null) return;

        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideMenu()
    {
        if (deathMenuUI == null) return;

        deathMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}