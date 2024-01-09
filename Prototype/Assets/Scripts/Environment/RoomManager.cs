using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static RoomManager rm;

    [SerializeField] private string startingRoomName;

    private GameObject currentRoom;
    public static event Action OnRoomChanged;

    public static GameObject GetCurrentRoom()
    {
        return rm.currentRoom;
    }

    public static void ChangeRoom(GameObject newRoom)
    {
        DestroyImmediate(rm.currentRoom); // Can't use normal Destroy(), because InteractionManager is getting the list of interactable on the same frame
        rm.currentRoom = Instantiate(newRoom);
        OnRoomChanged?.Invoke();
    }

    private void Start()
    {
        //Debug.Log("RoomManager Start");

        if (rm != null)
        {
            Debug.LogWarning("More than 1 RoomManager in the scene");
            return;
        }
        rm = this;

        //GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        //if (rooms.Length == 0)
        //{
        //    Debug.LogError("No gameobjects with tag \"Room\" present in the scene");
        //}
        //else
        //{
        //    currentRoom = rooms[0];
        //    if (rooms.Length > 1)
        //    {
        //        Debug.LogWarning("More than 1 gameobject with tag \"Room\" in the scene");
        //    }
        //}

        if (startingRoomName == "")
        {
            Debug.LogError("Starting room name has not been set");
            return;
        }

        currentRoom = RuntimeSaveManager.LoadRoom(startingRoomName);
    }
}
