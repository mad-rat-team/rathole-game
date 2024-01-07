using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(LootableItemSavable))]
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

    public object GetState()
    {
        SaveData saveData = new();
        saveData.item = inventoryItem;
        saveData.count = count;

        return saveData;
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        inventoryItem = saveData.item;
        count = saveData.count;
    }
}

//[RequireComponent(typeof(LootableItem))]
//public class LootableItemSavable : SavableRoomObject
//{
//    private LootableItem lootableItem;

//    private void Awake()
//    {
//        lootableItem = GetComponent<LootableItem>();
//    }

//    public override object GetSaveData() => lootableItem.GetSaveData();

//    public override void LoadSaveData(object saveData) => lootableItem.LoadSaveData(saveData);
//}
