using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Custom/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Color nameColor = new Color(255, 255, 255, 255);
    //public Sprite menuSprite;
    //public bool visibleInInventory = true;

    public string GetTMPColoredName()
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGBA(nameColor)}>{itemName}</color>";
    }
}
