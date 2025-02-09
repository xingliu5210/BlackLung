using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject controlWindow;
    [SerializeField] GameObject mainMenuButtonGroup;
    [SerializeField] GameObject mouseKeyPanel;
    [SerializeField] GameObject controllerPanel;
    [SerializeField] Button mouseKeyButton;
    [SerializeField] GameObject currentPanelOpen;
    [SerializeField] SceneController sceneManager;

    private void Start()
    {
        startScreen.SetActive(true);
        mainMenuButtonGroup.SetActive(true);
        controlWindow.SetActive(false);
    }

    public void CloseStartScreen()
    {
        startScreen.SetActive(false);
    }

    //Default opens the Mouse & Key Controlls when Controlls
    //window is opened
    public void OpenControlsWindow()
    {
        mainMenuButtonGroup.SetActive(false);
        mouseKeyButton.Select();
        OpenMouseKeyPanel();
        controlWindow.SetActive(true);
    }

    public void CloseControlsWindow()
    {
        mainMenuButtonGroup.SetActive(true);
        controlWindow.SetActive(false);
    }

    public void OpenMouseKeyPanel()
    {
        OpenPanel(mouseKeyPanel);
    }

    public void OpenControllerPanel()
    {
        OpenPanel(controllerPanel);
    }
    
    //Checks if there is a panel already open.
    //True -> close panel then open selected panel
    //False -> open selected panel
    private void OpenPanel(GameObject panel)
    {
        if (currentPanelOpen != null)
        {
            ClosePanel(currentPanelOpen);
        }
        currentPanelOpen = panel;
        currentPanelOpen.SetActive(true);
    }

    private void ClosePanel(GameObject panel)
    {
        if(currentPanelOpen != null)
        {
            panel.SetActive(false);
        }
    }

    public void StartGameAndLoad()
    {
        sceneManager.OnLoadFirstScene(); // Load the game scene
        StartCoroutine(LoadGameAfterSceneLoad());
    }

    private IEnumerator LoadGameAfterSceneLoad()
    {
        // Wait for the next frame to ensure the scene is loaded
        yield return new WaitForSeconds(0.1f);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject ally = GameObject.FindGameObjectWithTag("Dog");

        if (player != null && ally != null)
        {
            bool loaded = SaveSystem.LoadGame(player.transform, ally.transform);
            if (loaded)
            {
                Debug.Log("Game Loaded after scene transition!");
            }
            else
            {
                Debug.LogWarning("No save file found!");
            }
        }
        else
        {
            Debug.LogError("Player or Ally object not found in the gameplay scene.");
        }
    }

    public void StartNewGame()
    {
        SaveSystem.DeleteSave(); // Clears previous save data
        SaveSystem saveSystem = FindObjectOfType<SaveSystem>(); // Find SaveSystem instance

        if (saveSystem != null)
        {
            saveSystem.ResetGame(); // Reset game state
        }
        else
        {
            Debug.LogError("SaveSystem not found in the scene!");
        }

        sceneManager.OnLoadFirstScene(); // Load first scene
    }

    private IEnumerator ResetGameAfterSceneLoad()
    {
        SaveSystem saveSystem = FindObjectOfType<SaveSystem>(); // Find SaveSystem instance
        yield return new WaitForSeconds(0.1f); // Ensure the scene is loaded

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bo = GameObject.FindGameObjectWithTag("Dog");

        if (player != null && bo != null)
        {
            saveSystem.ResetGame();
        }
        else
        {
            Debug.LogError("Player or Ally not found in the scene!");
        }
    }
}
