using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //[SerializeField] private Grid grid;

    private static RoomManager rm;

    private GameObject currentRoom;
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

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        if (rooms.Length == 0)
        {
            Debug.LogError("No gameobjects with tag \"Room\" present in the scene");
        }
        else
        {
            currentRoom = rooms[0];
            if(rooms.Length > 1)
            {
                Debug.LogWarning("More than 1 gameobject with tag \"Room\" in the scene");
            }
        }


        //if(currentRoom == null)
        //{
        //    Debug.LogWarning("No current room is set in RoomManager");
        //}
    }
}
