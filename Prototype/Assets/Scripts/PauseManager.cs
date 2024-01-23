using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager pm;
    private bool paused = false;

    public static void SetPaused(bool newPaused)
    {
        pm.paused = newPaused;
        Time.timeScale = newPaused ? 0f : 1f;
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
    }

    private void Update()
    {
        if (InputManager.GetButtonDown(InputManager.InputButton.Pause))
        {
            SetPaused(!paused);
        }
    }
}
