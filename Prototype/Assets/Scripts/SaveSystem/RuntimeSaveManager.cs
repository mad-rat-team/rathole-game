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

    public static void SaveRoom(GameObject room)
    {
        //rsm.saveSystem.SaveRoomToSystem(room, )
    }

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
