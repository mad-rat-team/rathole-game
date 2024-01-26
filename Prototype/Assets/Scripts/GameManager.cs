using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private static GameManager gm;

    //const string MainSceneName = "MainLocation";
    const string MainSceneName = "Main";
    const string MainMenuSceneName = "MainMenu";

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void LoadMainScene()
    {
        SceneManager.LoadScene(MainSceneName);
    }

    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }

    public static GameObject GetPlayer()
    {
        return gm.player;
    }

    private void Awake()
    {
        if (gm != null)
        {
            Debug.LogWarning("More than 1 GameManager in the scene");
            return;
        }
        gm = this;
    }
}
