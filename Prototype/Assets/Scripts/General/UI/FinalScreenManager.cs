using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    public void HandleExit()
    {
        GameManager.ExitGame();
    }

    public void HandleMainMenu()
    {
        GameManager.LoadMainMenuScene();
    }
}
