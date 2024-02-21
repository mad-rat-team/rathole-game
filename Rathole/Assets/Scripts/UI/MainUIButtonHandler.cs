using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIButtonHandler : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 1f;

    public void HandleResume()
    {
        PauseManager.SetPaused(false);
        PauseManager.SetPauseMenuActive(false);
    }

    public void HandleMainMenu()
    {
        PauseManager.SetPauseMenuActive(false);
        PauseManager.PauseForSecondsAndPerformAction(fadeOutTime, GameManager.LoadMainMenuScene);
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutTime, 0f, true);
        SoundManager.FadeOutSoundtrack(fadeOutTime);
    }

    public void HandleLoadLastSave()
    {
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        PauseManager.SetPauseMenuActive(false);
        PauseManager.PauseForSecondsAndPerformAction(fadeOutTime, GameManager.LoadMainScene);
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutTime, 0f, true);
        SoundManager.FadeOutSoundtrack(fadeOutTime);
    }

    public void HandleLoadLastSaveFromDeathScreen()
    {
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        HandleLoadLastSave();
    }

    public void HandleExit()
    {
        GameManager.ExitGame();
    }
}
