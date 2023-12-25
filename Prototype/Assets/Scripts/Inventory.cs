using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Serializable] private struct StartingItem
    {
        public InventoryItem item;
        public int count;
    }

    [SerializeField] private StartingItem[] startingItems;

    private Dictionary<InventoryItem, int> itemCounts = new();

    public int GetItemCount(InventoryItem item)
    {
        int count;
        bool hasItem = itemCounts.TryGetValue(item, out count);
        return hasItem ? count : 0;
    }

    public void StoreItems(InventoryItem item, int count)
    {
        if (itemCounts.ContainsKey(item))
        {
            itemCounts[item] += count;
        }
        else
        {
            itemCounts[item] = count;
        }
    }

    private void Start()
    {
        foreach (StartingItem startingItem in startingItems)
        {
            StoreItems(startingItem.item, startingItem.count);
        }
    }
}
