using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    private bool active = true;

    public void HandleExit()
    {
        if (!active) return;
        GameManager.ExitGame();
    }

    public void HandleMainMenu()
    {
        if (!active) return;
        active = false;
        IEnumerator waitAndLoadMainMenu()
        {
            yield return new WaitForSeconds(fadeOutDuration);
            GameManager.LoadMainMenuScene();
        };
        StartCoroutine(waitAndLoadMainMenu());
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutDuration, 0f, false);
    }

    private void Start()
    {
        ScreenEffectManager.Fade(Color.black, new Color(0, 0, 0, 0), fadeInDuration, 0f, false);
    }
}
