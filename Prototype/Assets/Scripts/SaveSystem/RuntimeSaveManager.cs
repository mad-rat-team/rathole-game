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
        //GameObject player = GameManager.GetPlayer();
        //rsm.saveSystem.SavePlayerData(RoomManager.GetCurrentRoomName(), player.transform.position, player.GetComponent<Inventory>().GetState());
        rsm.saveSystem.SavePlayerState(GameManager.GetPlayer().GetComponent<PlayerData>().GetState());
        //NOTE: GetComponent() here is not good, it would be better to have a component on player that stores other Player... components
        rsm.saveSystem.SaveToDisk();
    }

    public static void LoadGame()
    {
        rsm.saveSystem.LoadFromDisk();
        //RoomManager.ChangeRoomWithoutSaving(rsm.saveSystem.GetSavedRoomName());
        //GameObject player = GameManager.GetPlayer();
        //player.transform.position = rsm.saveSystem.GetSavedPlayerPosition();
        //player.GetComponent<Inventory>().LoadState(rsm.saveSystem.GetPlayerInventoryState());
        GameManager.GetPlayer().GetComponent<PlayerData>().LoadState(rsm.saveSystem.GetPlayerState());
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

        //SaveSystem.CreateNewSaveFile(); // PH: This should be handled by main menu
        saveSystem = new SaveSystem(SaveSystem.SaveFileType.Existing);
    }

    private void Start()
    {
        LoadGame();
    }
}
