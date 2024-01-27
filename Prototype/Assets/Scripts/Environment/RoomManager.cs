using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    const string enemyTag = "Enemy";

    private static RoomManager rm;

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
        // Can't use normal Destroy(), because InteractionManager is getting the list of interactable on the same frame
        if (rm.currentRoom != null) DestroyImmediate(rm.currentRoom);
        rm.currentRoom = RuntimeSaveManager.LoadRoom(newRoomName);
        rm.currentRoomName = newRoomName;
        OnRoomChanged?.Invoke();
    }

    public static int GetCurrentRoomEnemyCount()
    {
        int count = 0;
        foreach (Transform child in rm.currentRoom.transform)
        {
            if (child.CompareTag(enemyTag))
            {
                count++;
            }
        }
        return count;
    }

    private void Awake()
    {
        if (rm != null)
        {
            Debug.LogWarning("More than 1 RoomManager in the scene");
            return;
        }
        rm = this;
    }
}
