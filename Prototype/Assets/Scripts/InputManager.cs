using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public enum InputButton
    {
        Interact,
        Pause,
        Attack
    }

    private class InputButtonInfo
    {
        public KeyCode keyCode;
        public bool invokeWhenPaused;
    }

    private static Dictionary<InputButton, InputButtonInfo> inputbuttonInfos = new Dictionary<InputButton, InputButtonInfo>
    {
        {
            InputButton.Interact,
            new InputButtonInfo
            {
                keyCode = KeyCode.E,
                invokeWhenPaused = false
            }
        },
        {
            InputButton.Attack,
            new InputButtonInfo
            {
                keyCode = KeyCode.Mouse0,
                invokeWhenPaused = false
            }
        },
        {
            InputButton.Pause,
            new InputButtonInfo
            {
                keyCode = KeyCode.Escape,
                invokeWhenPaused = true
            }
        }
    };

    public static bool GetButtonDown(InputButton inputButton)
    {
        InputButtonInfo info = inputbuttonInfos[inputButton];
        return (info.invokeWhenPaused || !PauseManager.GetPaused()) && Input.GetKeyDown(info.keyCode);
    }
    
    public static bool GetButton(InputButton inputButton)
    {
        InputButtonInfo info = inputbuttonInfos[inputButton];
        return (info.invokeWhenPaused || !PauseManager.GetPaused()) && Input.GetKey(info.keyCode);
    }

    /// <summary>
    /// Gets input move dir.
    /// </summary>
    /// <returns>
    /// <b>Non-iso-vector</b> of movement.
    /// </returns>
    public static Vector2 GetInputMoveDir()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

}
