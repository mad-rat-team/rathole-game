using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : Interactable
{
    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private int count = 1;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        interactionAgent.Inventory.StoreItems(inventoryItem, count);
        Destroy(gameObject);
    }
}
