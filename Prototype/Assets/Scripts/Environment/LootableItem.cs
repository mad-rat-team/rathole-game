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
        public SerializableVector3 position;
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
        saveData.itemName = inventoryItem.name;
        saveData.count = count;
        saveData.position = new SerializableVector3(transform.position);

        return saveData;
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        //inventoryItem = saveData.item;
        inventoryItem = Resources.Load<InventoryItem>(saveData.itemName);
        count = saveData.count;
        transform.position = saveData.position.GetVector3();
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
