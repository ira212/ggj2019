using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName: "Gameplay");
    }

    public void Options()
    {
        SceneManager.LoadScene(sceneName: "TweakablesMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(sceneName: "MainMenu");
    }
    public void Instructions()
    {
        SceneManager.LoadScene(sceneName: "Instructions");
    }
}
