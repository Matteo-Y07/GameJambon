using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESCAPE PRESSED");

            if (isPaused)
                Resume();
            else
                ShowMenu();
        }
    }

    public void ShowMenu()
    {
        if (pauseMenuUI == null) return;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        if (pauseMenuUI == null) return;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartFromCheckpoint()
    {
        Time.timeScale = 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();

            if (respawn != null)
            {
                Vector3 pos = GameManager.instance.GetCheckpoint(respawn.GetStartPosition());
                player.transform.position = pos;
            }
        }

        Resume();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}