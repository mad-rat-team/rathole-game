using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class RuntimeSaveManager : MonoBehaviour
{
    [SerializeField] private float loadGameFadeInRestDuration = 0.5f;
    [SerializeField] private float loadGameFadeInDuration = 1f;

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
        rsm.saveSystem.SavePlayerState(GameManager.GetPlayer().GetComponent<PlayerData>().GetState());
        //NOTE: GetComponent() here is not good, it would be better to have a component on player that stores other "Player..." components
        rsm.saveSystem.SaveToDisk();
    }

    public static void LoadGame()
    {
        rsm.saveSystem.LoadFromDisk();
        GameManager.GetPlayer().GetComponent<PlayerData>().LoadState(rsm.saveSystem.GetPlayerState());
        ScreenEffectManager.Fade(Color.black, new Color(0, 0, 0, 0), rsm.loadGameFadeInRestDuration, rsm.loadGameFadeInDuration);
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

        saveSystem = new SaveSystem(SaveSystem.SaveFileType.Existing);
    }

    private void Start()
    {
        LoadGame();
    }
}
