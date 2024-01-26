using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //private SaveSystem saveSystem;
    [SerializeField] private Button loadGameButton;

    public void HandleNewGame()
    {
        SaveSystem.CreateNewSaveFile();
        GameManager.LoadMainScene();
    }

    public void HandleLoadGame()
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

    //private void Awake()
    //{
    //    saveSystem = new SaveSystem(SaveSystem.SaveFileType.Existing);
    //}

    private void Start()
    {
        loadGameButton.interactable = SaveSystem.SaveFileExists();
    }
}
