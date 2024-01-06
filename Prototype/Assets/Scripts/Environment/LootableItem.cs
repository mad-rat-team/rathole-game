using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : Interactable, ISavable
{
    private class SaveData
    {
        public InventoryItem item;
        public int count;
    }

    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private int count = 1;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        interactionAgent.Inventory.StoreItems(inventoryItem, count);
        Destroy(gameObject);
    }

    public object GetSaveData()
    {
        SaveData saveData = new();
        saveData.item = inventoryItem;
        saveData.count = count;

        return saveData;
    }

    public void LoadSaveData(object saveDataObject)
    {
        SaveData saveData = (SaveData)saveDataObject;
        inventoryItem = saveData.item;
        count = saveData.count;
    }
}
