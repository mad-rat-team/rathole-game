using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIButtonHandler : MonoBehaviour
{
    public void HandleMainMenu()
    {
        GameManager.LoadMainMenuScene();
    }

    public void HandleLoadLastSave(float fadeOutTime)
    {
        if (!SaveSystem.SaveFileExists())
        {
            throw new System.Exception("Save file does not exist");
        }
        PauseManager.TogglePauseMenu(false);
        PauseManager.PauseForSecondsAndPerformAction(fadeOutTime, GameManager.LoadMainScene);
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutTime, 0f, true);
    }

    public void HandleExit()
    {
        GameManager.ExitGame();
    }
}
