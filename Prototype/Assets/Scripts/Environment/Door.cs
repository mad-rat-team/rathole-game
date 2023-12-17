using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private GameObject nextRoom;
    [SerializeField] private int doorId; //ID of the door
    [SerializeField] private int nextRoomDoorId; //ID of a door in the nextRoom to which the player will be teloported
    [SerializeField] private Vector2 playerTeleportOffset;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        RoomManager.ChangeRoom(nextRoom);
        foreach(Door nextRoomDoor in RoomManager.GetCurrentRoom().GetComponentsInChildren<Door>())
        {
            if(nextRoomDoor.doorId == nextRoomDoorId)
            {
                // NOTE: Maybe change agent's position not directly but through Movement
                interactionAgent.transform.position = nextRoomDoor.transform.position + (Vector3)nextRoomDoor.playerTeleportOffset;
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3)playerTeleportOffset, 0.125f);
    }
}
