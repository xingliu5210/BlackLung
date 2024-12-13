using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OnLoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLoadFirstScene()
    {
        SceneManager.LoadScene(1); 
    }

    public void OnExitGame()
    {
        Application.Quit();
    }
}
