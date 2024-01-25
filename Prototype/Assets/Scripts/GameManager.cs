using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //const string MainSceneName = "MainLocation";
    const string MainSceneName = "DoorsTest";
    const string MainMenuSceneName = "MainMenu";

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
