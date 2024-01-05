using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gm;

    public enum RoomObjectType
    {
        //Door,
        LootableItem = 0,
    }

    public class RoomObjectData
    {
        public string name;
        public RoomObjectType type;
        public bool lootableItemLooted;
    }

    public class RoomData
    {
        public string name;
        public bool cleared;
        public RoomObjectData[] objects;
    }

    public static void SaveRoom()
    {

    }

    private void Start()
    {
        if (gm != null)
        {
            Debug.LogWarning("More than 1 GameManager in the scene");
        }
        gm = this;
    }
}
