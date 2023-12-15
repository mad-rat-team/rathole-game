using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] Object currentRoom; //PH
    [SerializeField] Object nextRoom;
    [SerializeField] Grid grid; //PH
    public void Interact()
    {
        Destroy(currentRoom);
        Instantiate(nextRoom, grid.transform);
    }
}
