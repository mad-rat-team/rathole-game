using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIHandler : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

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

    private void Start()
    {
        sfxSlider.value = SoundManager.GetSfxVolume();
        musicSlider.value = SoundManager.GetMusicVolume();
    }
}
