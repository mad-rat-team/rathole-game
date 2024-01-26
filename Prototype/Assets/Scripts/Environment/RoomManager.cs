using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager rm;

    [SerializeField] private string startingRoomName;

    private GameObject currentRoom;
    private string currentRoomName;
    public static event Action OnRoomChanged;

    public static GameObject GetCurrentRoom()
    {
        return rm.currentRoom;
    }

    public static string GetCurrentRoomName()
    {
        return rm.currentRoomName;
    }

    public static void ChangeRoom(string newRoomName)
    {
        RuntimeSaveManager.SaveRoom(rm.currentRoom, rm.currentRoomName);
        ChangeRoomWithoutSaving(newRoomName);
    }

    public static void ChangeRoomWithoutSaving(string newRoomName)
    {
        DestroyImmediate(rm.currentRoom); // Can't use normal Destroy(), because InteractionManager is getting the list of interactable on the same frame
        rm.currentRoom = RuntimeSaveManager.LoadRoom(newRoomName);
        rm.currentRoomName = newRoomName;
        OnRoomChanged?.Invoke();
    }

    private void Awake()
    {
        if (rm != null)
        {
            Debug.LogWarning("More than 1 RoomManager in the scene");
            return;
        }
        rm = this;

        if (startingRoomName == "")
        {
            Debug.LogError("Starting room prefab resource path has not been set");
            return;
        }
    }

    private void Start()
    {
        currentRoom = RuntimeSaveManager.LoadRoom(startingRoomName);
        currentRoomName = startingRoomName;
    }
}
