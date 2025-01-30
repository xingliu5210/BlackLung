using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Reference to the pause menu UI panel.")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject controlsWindowUI;
    [SerializeField] private GameObject controllerPanel;
    [SerializeField] private GameObject mouseKeyPanel;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject controlsButton;
    [SerializeField] private GameObject quitButton;

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
            exitPanel.SetActive(false);
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

    public void Controls()
    {
        // Show the Controls Window and hide the main pause menu
        Debug.Log("Controls() called"); // Debug message
        pauseMenuUI.SetActive(false);
        controllerPanel.SetActive(false);
        mouseKeyPanel.SetActive(true);
        controlsWindowUI.SetActive(true);
    }

    public void ControllerPanel()
    {
        controllerPanel.SetActive(true);
        mouseKeyPanel.SetActive(false);
    }

    public void MouseKeyPanel()
    {
        controllerPanel.SetActive(false);
        mouseKeyPanel.SetActive(true);
    }

    public void CloseControls()
    {
        // Hide the Controls Window and return to the pause menu
        Debug.Log("CloseControls() called"); // Debug message
        pauseMenuUI.SetActive(true);
        controlsWindowUI.SetActive(false);
    }

    public void ExitPanel()
    {
        exitPanel.SetActive(true);
        controlsButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void CancelExit()
    {
        exitPanel.SetActive(false);
        controlsButton.SetActive(true);
        quitButton.SetActive(true);
    }

}
