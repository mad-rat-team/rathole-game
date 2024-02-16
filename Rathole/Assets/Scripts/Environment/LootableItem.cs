using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LootableItemSavable))]
public class LootableItem : Interactable, ISavable
{
    [System.Serializable]
    private class SaveData
    {
        public string itemName;
        public int count;
        //public SerializableVector3 position;
    }

    [Header("LootableItem")]
    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private int count = 1;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        interactionAgent.Inventory.StoreItems(inventoryItem, count);
        if(count == 1)
        {
            ScreenEffectManager.ShowMessage($"{inventoryItem.GetTMPColoredName()} obtained");
        }
        else
        {
            ScreenEffectManager.ShowMessage($"X{count} {inventoryItem.GetTMPColoredName()} obtained");
        }
        Destroy(gameObject);
    }

    public object GetState()
    {
        SaveData saveData = new();
        saveData.itemName = inventoryItem.name;
        saveData.count = count;
        //saveData.position = new SerializableVector3(transform.position);

        return saveData;
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        //inventoryItem = saveData.item;
        inventoryItem = Resources.Load<InventoryItem>(saveData.itemName);
        count = saveData.count;
        //transform.position = saveData.position.GetVector3();
    }
}