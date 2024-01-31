using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private static PauseManager pm;
    private bool paused = false;

    private bool pausedForSeconds = false;
    private float unpauseTime;
    private System.Action onUnpauseAction;

    private bool manualPauseAllowed = true;

    //public static void SetManualPauseAllowed(bool manualPauseAllowed)
    //{
    //    pm.manualPauseAllowed = manualPauseAllowed;
    //}

    public static void PauseForSecondsAndPerformAction(float duration, System.Action action)
    {
        pm.unpauseTime = Time.unscaledTime + duration;
        SetPaused(true);
        pm.pausedForSeconds = true;
        pm.onUnpauseAction = action;
    }

    public static void SetPaused(bool newPaused)
    {
        if (pm.pausedForSeconds) return;

        pm.paused = newPaused;
        Time.timeScale = newPaused ? 0f : 1f;
        //pm.pauseMenu.SetActive(newPaused);
    }

    public static void TogglePauseMenu(bool active)
    {
        if (pm.pausedForSeconds) return;

        pm.pauseMenu.SetActive(active);
    }

    public static bool GetPaused()
    {
        return pm.paused;
    }

    private void Awake()
    {
        if (pm != null)
        {
            Debug.LogWarning("More than 1 PauseManager in the scene");
            return;
        }
        pm = this;

        TogglePauseMenu(false);
    }

    private void Update()
    {
        if (pausedForSeconds)
        {
            if (Time.unscaledTime >= unpauseTime)
            {
                pausedForSeconds = false;
                SetPaused(false);
                onUnpauseAction?.Invoke();
            }
        }

        if (InputManager.GetButtonDown(InputManager.InputButton.Pause) && manualPauseAllowed)
        {
            SetPaused(!paused);
            TogglePauseMenu(paused);
        }
    }
}
