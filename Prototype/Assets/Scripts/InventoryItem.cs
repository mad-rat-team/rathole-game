using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Custom/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public bool visibleInInventory = true;
}
