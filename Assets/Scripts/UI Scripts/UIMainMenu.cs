using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject controlWindow;
    [SerializeField] GameObject mainMenuButtonGroup;
    [SerializeField] GameObject mouseKeyPanel;
    [SerializeField] GameObject controllerPanel;
    private GameObject currentPanelOpen;

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
    public void OpenControlsWindow()
    {
        mainMenuButtonGroup.SetActive(false);
        controlWindow.SetActive(true);
        OpenPanel(mouseKeyPanel);
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
}
