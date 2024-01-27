using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathScreen;

    private static GameManager gm;

    const string MainSceneName = "Main";
    const string MainMenuSceneName = "MainMenu";

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
        SceneManager.LoadScene(MainSceneName);
    }

    public static void LoadMainMenuScene()
    {
        Time.timeScale = 1f;
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
