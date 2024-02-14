using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button loadGameButton;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private GameObject controlsParent;
    [SerializeField] private GameObject creditsParent;

    private bool active = true;

    public void HandleNewGame()
    {
        if (!active) return;
        SaveSystem.CreateNewSaveFile();
        FadeAndLoad();
    }

    public void HandleLoadGame()
    {
        if (!active) return;
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        FadeAndLoad();
    }

    public void HandleControls()
    {
        controlsParent.SetActive(true);
    }

    public void HandleControlsBack()
    {
        controlsParent.SetActive(false);
    }

    public void HandleExit()
    {
        if (!active) return;
        GameManager.ExitGame();
    }

    public void HandleCredits()
    {
        creditsParent.SetActive(true);
    }

    public void HandleCreditsBack()
    {
        creditsParent.SetActive(false);
    }

    private void FadeAndLoad()
    {
        active = false;
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutDuration, 0f, false);
        IEnumerator waitAndLoad()
        {
            yield return new WaitForSeconds(fadeOutDuration);
            GameManager.LoadMainScene();
        };
        StartCoroutine(waitAndLoad());
    }

    private void Start()
    {
        loadGameButton.interactable = SaveSystem.SaveFileExists();
        ScreenEffectManager.Fade(Color.black, new Color(0, 0, 0, 0), fadeInDuration, 0f, false);
    }
}
