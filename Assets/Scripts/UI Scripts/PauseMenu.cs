using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Reference to the pause menu UI panel.")]
    [SerializeField] private GameObject pauseMenuUI;

    private bool isPaused = false;

    /// <summary>
    /// Indicates whether the game is currently paused.
    /// </summary>
    public bool IsPaused => isPaused;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the pause menu UI is hidden at the start.
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PauseMenuUI is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Pauses the game and shows the pause menu.
    /// </summary>
    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("PauseMenuUI is not assigned in the inspector.");
        }

        Time.timeScale = 0f; // Freeze game time.
        isPaused = true;
        Debug.Log("Game Paused.");
    }

    /// <summary>
    /// Resumes the game and hides the pause menu.
    /// </summary>
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        Time.timeScale = 1f; // Resume game time.
        isPaused = false;
        Debug.Log("Game Resumed.");
    }

    /// <summary>
    /// Toggles the pause menu state.
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Quits the game (for builds).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
