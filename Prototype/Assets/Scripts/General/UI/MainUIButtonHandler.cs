using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIButtonHandler : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 1f;

    public void HandleMainMenu()
    {
        PauseManager.TogglePauseMenu(false);
        PauseManager.PauseForSecondsAndPerformAction(fadeOutTime, GameManager.LoadMainMenuScene);
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutTime, 0f, true);
    }

    public void HandleLoadLastSave()
    {
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        PauseManager.TogglePauseMenu(false);
        PauseManager.PauseForSecondsAndPerformAction(fadeOutTime, GameManager.LoadMainScene);
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutTime, 0f, true);
    }

    public void HandleLoadLastSaveFromDeathScreen()
    {
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        GameManager.LoadMainScene();
    }

    public void HandleExit()
    {
        GameManager.ExitGame();
    }
}
