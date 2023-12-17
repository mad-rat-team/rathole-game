using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //[SerializeField] private Grid grid;
    [SerializeField] private GameObject currentRoom;

    private static RoomManager rm;

    public static event Action OnRoomChanged;

    public static GameObject GetCurrentRoom()
    {
        return rm.currentRoom;
    }

    public static void ChangeRoom(GameObject newRoom)
    {
        DestroyImmediate(rm.currentRoom); // Can't use normal Destroy(), because InteractionManager is getting the list of interactable on the same frame
        //rm.currentRoom = Instantiate(newRoom, rm.grid.transform);
        rm.currentRoom = Instantiate(newRoom);
        OnRoomChanged?.Invoke();
    }

    private void Start()
    {
        if (rm != null)
        {
            Debug.LogWarning("More than 1 RoomManager in the scene");
        }

        rm = this;

        if(currentRoom == null)
        {
            Debug.LogWarning("No current room is set in RoomManager");
        }
    }
}
