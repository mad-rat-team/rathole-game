using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Health))]
public class PlayerData : MonoBehaviour, ISavable
{
    private Inventory inventory;

    [System.Serializable]
    public class State
    {
        public SerializableVector3 position;
        public string currentRoomName;
        public int health;
        public object inventoryState;
    }

    public object GetState()
    {
        State state = new();
        state.position = new SerializableVector3(transform.position);
        state.currentRoomName = RoomManager.GetCurrentRoomName();
        state.inventoryState = inventory.GetState();
        state.health = GetComponent<Health>().GetHealthAmount();

        return state;
    }

    public void LoadState(object state)
    {
        State castState = (State)state;
        transform.position = castState.position.GetVector3();
        RoomManager.ChangeRoomWithoutSaving(castState.currentRoomName);
        inventory.LoadState(castState.inventoryState);
        GetComponent<PlayerCombat>().ResetHasWeapon();
        GetComponent<Health>().SetHealth(castState.health);
    }

    public void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
}
