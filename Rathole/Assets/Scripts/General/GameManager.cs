using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathScreen;

    private static GameManager gm;

    const string mainSceneName = "Main";
    const string mainMenuSceneName = "MainMenu";
    const string endingCutsceneScene = "EndingCutscene";
    const string finalScreenSceneName = "FinalScreen";

    public static void HandleDeath()
    {
        PauseManager.SetPaused(true);
        gm.player.SetActive(false);
        gm.deathScreen.SetActive(true);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void LoadMainScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainSceneName);
    }

    public static void LoadMainMenuScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public static void LoadFinalScreenScene()
    {
        SceneManager.LoadScene(finalScreenSceneName);
    }

    public static void LoadEndingCutsceneScene()
    {
        SceneManager.LoadScene(endingCutsceneScene);
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
