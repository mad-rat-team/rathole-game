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

    public static void SaveRoom(GameObject room, string roomName)
    {
        rsm.saveSystem.SaveRoomToSystem(room, roomName);
    }

    public static void SaveGame()
    {
        SaveRoom(RoomManager.GetCurrentRoom(), RoomManager.GetCurrentRoomName());
        rsm.saveSystem.SaveToDisk();
    }

    public static void LoadGame()
    {
        rsm.saveSystem.LoadFromDisk();
        RoomManager.ChangeRoomWithoutSaving(RoomManager.GetCurrentRoomName()); //PH
    }

    private void Awake()
    {
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
