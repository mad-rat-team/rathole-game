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

    //This was supposed to be fancy, but c# does not appear to have the functionality I wanted with events

    //private static InputManager im;

    //public static event Action OnInteractPressed;
    //public static event Action OnPausePressed;

    //private enum InputType
    //{
    //    Press,
    //    Release
    //}

    //private class InputEvent
    //{
    //    public Action inputEventHandler;
    //    public KeyCode keyCode;
    //    public InputType inputType;
    //    public bool invokeWhilePaused;
    //}

    //private static InputEvent[] inputEvents;

    //private void Awake()
    //{
    //    //if (im != null)
    //    //{
    //    //    Debug.LogWarning("More than 1 InputManager in the scene");
    //    //    return;
    //    //}
    //    //im = this;

    //    inputEvents = new InputEvent[] {
    //        new InputEvent
    //        {
    //            inputEventHandler = OnInteractPressed,
    //            keyCode = KeyCode.E,
    //            inputType = InputType.Press,
    //            invokeWhilePaused = false
    //        },
    //        new InputEvent
    //        {
    //            inputEventHandler = OnPausePressed,
    //            keyCode = KeyCode.Escape,
    //            inputType = InputType.Press,
    //            invokeWhilePaused = true
    //        }
    //    };
    //}

    //private void Update()
    //{
    //foreach (InputEvent inputEvent in inputEvents)
    //{
    //    if (!inputEvent.invokeWhilePaused && PauseManager.GetPaused()) continue;
    //    if (inputEvent.inputType == InputType.Press ? Input.GetKeyDown(inputEvent.keyCode) : Input.GetKeyUp(inputEvent.keyCode))
    //    {
    //        //Debug.Log(OnPausePressed.GetInvocationList().Length);
    //        Debug.Log(inputEvent.keyCode);
    //        inputEvent.inputEventHandler?.Invoke();
    //    }
    //}
    //}
}
