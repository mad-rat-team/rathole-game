using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class RuntimeSaveManager : MonoBehaviour
{
    private static RuntimeSaveManager rsm;

    private SaveSystem saveSystem;
    
    public static GameObject LoadRoom(string roomName)
    {
        return rsm.saveSystem.LoadRoom(roomName);
    }

    //private static void LazyInit() // Idk how to do this nicer
    //{
    //    if (saveSystem != null) return;
    //}

    //private void Init()
    //{
    //    rsm.saveSystem = new SaveSystem();
    //    rsm.saveSystem.LoadGameDataFromInitialSave();
    //}

    private void Awake()
    {
        //Debug.Log("RuntimeSaveManager Awake");

        if (rsm != null)
        {
            if (rsm != this)
            {
                Debug.LogWarning("More than 1 RuntimeSaveManager in the scene");
            }
            return;
        }

        rsm = this;
        SaveSystem.CreateNewSaveFile(); // PH: This should be handled by main menu
        saveSystem = new SaveSystem(SaveSystem.SaveFileType.Existing);
    }
}
