using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
{
    //[Serializable] private struct StartingItem
    //{
    //    public InventoryItem item;
    //    public int count;
    //}

    //[SerializeField] private StartingItem[] startingItems;

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

    //private void Awake()
    //{
    //    foreach (StartingItem startingItem in startingItems)
    //    {
    //        if(startingItem.item == null)
    //        {
    //            Debug.LogWarning("StartingItem cannot be null");
    //            continue;
    //        }
    //        StoreItems(startingItem.item, startingItem.count);
    //    }
    //}

    public object GetState()
    {
        Dictionary<string, int> itemCountsState = new();
        foreach(var pair in itemCounts)
        {
            itemCountsState[pair.Key.name] = pair.Value;
        }

        return itemCountsState;
    }

    public void LoadState(object state)
    {
        itemCounts = new();
        foreach(var pair in (Dictionary<string, int>)state)
        {
            itemCounts[Resources.Load<InventoryItem>(pair.Key)] = pair.Value;
        }
    }

    public static object GetEmptyState()
    {
        return new Dictionary<string, int>();
    }
}
