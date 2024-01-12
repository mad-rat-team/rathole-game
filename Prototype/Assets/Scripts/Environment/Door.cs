using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private string nextRoomName;
    [SerializeField] private int doorId; //ID of the door
    [SerializeField] private int nextRoomDoorId; //ID of a door in the nextRoom to which the player will be teloported
    [SerializeField] private Vector2 playerTeleportOffset;

    [Header(header: "Lock")]
    [SerializeField] private bool hasALock;
    [SerializeField] InventoryItem requiredKey;

    private bool isLocked = true;

    public void SetIsLocked(bool newIsLocked)
    {
        isLocked = newIsLocked; //TODO: save this somehow so it stays unlocked when reentering the room
    }

    public override void Interact(PlayerInteractions interactionAgent)
    {
        if(hasALock && isLocked)
        {
            if(interactionAgent.Inventory.GetItemCount(requiredKey) <= 0)
            {
                Debug.Log("You don's have the required key"); //TODO: Add some player feedback
                return;
            }
            SetIsLocked(false);
        }

        //RoomManager.ChangeRoom(nextRoom);
        RoomManager.ChangeRoom(nextRoomName);

        Vector2 nextRoomPlayerPos = Vector2.zero;
        Door[] nextRoomDoors = RoomManager.GetCurrentRoom().GetComponentsInChildren<Door>();
        bool foundDoor = false;
        foreach (Door nextRoomDoor in nextRoomDoors)
        {
            if(nextRoomDoor.doorId == nextRoomDoorId)
            {
                nextRoomPlayerPos = nextRoomDoor.transform.position + (Vector3)nextRoomDoor.playerTeleportOffset;
                foundDoor = true;
                break;
            }
        }

        if(!foundDoor)
        {
            Debug.LogError($"Could not find the door with ID {nextRoomDoorId}");
            if(nextRoomDoors.Length != 0)
            {
                nextRoomPlayerPos = nextRoomDoors[0].transform.position + (Vector3)nextRoomDoors[0].playerTeleportOffset;
            }
        }

        // NOTE: Maybe change agent's position not directly but through Movement
        interactionAgent.transform.position = nextRoomPlayerPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)playerTeleportOffset, 0.125f);
    }
}
