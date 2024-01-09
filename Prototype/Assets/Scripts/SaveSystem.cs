using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private static string saveFilePath = Application.dataPath + "/TempSaves"; // TODO: Replace with persistentDataPath
    private static string initialSavePath = Application.dataPath + "/PrebakedData/initialSave" + saveExtension;
    private static string saveExtension = ".save";

    private BinaryFormatter binFormatter = new();
    //private Dictionary<string, RoomData> roomDataDict = new();
    private GameData gameData = new();

    [Serializable]
    public class RoomData
    {
        //public string name;
        //public bool cleared;
        public string prefabResourcePath;
        public SavableRoomObject.RoomObjectData[] objects;
    }

    [Serializable]
    public class GameData
    {
        // TODO: Player info
        public Dictionary<string, RoomData> roomDataDict;
        
        public GameData()
        {
            roomDataDict = new();
        }
    }

    public void SaveRoomToSystem(GameObject room, string roomName, string roomPrefabResourcePath)
    {
        RoomData roomData = new();
        roomData.prefabResourcePath = roomPrefabResourcePath;
        SavableRoomObject[] savables = SavableRoomObject.GetSavableRoomObjects(room);
        roomData.objects = new SavableRoomObject.RoomObjectData[savables.Length];
        for (int i = 0; i < savables.Length; i++)
        {
            roomData.objects[i] = savables[i].GetData();
        }

        gameData.roomDataDict[roomName] = roomData;
    }

    public GameObject LoadRoom(string roomName)
    {
        RoomData roomData;
        try
        {
            roomData = gameData.roomDataDict[roomName];
        }
        catch
        {
            foreach (var key in gameData.roomDataDict.Keys) Debug.Log(key);
            throw new Exception("Room with given name does not exist");
        }

        //A lot of path-related exceptions can be thrown here
        GameObject room = GameObject.Instantiate((GameObject)Resources.Load(roomData.prefabResourcePath));

        foreach (SavableRoomObject.RoomObjectData objectData in roomData.objects)
        {
            GameObject roomObject = GameObject.Instantiate((GameObject)Resources.Load(objectData.prefabResourcePath), room.transform);
            roomObject.GetComponent<SavableRoomObject>().LoadState(objectData.state);
        }

        return room;
    }

    public void SaveGameDataToInitialSave()
    {
        SaveGameDataToFile(initialSavePath);
    }

    public void LoadGameDataFromInitialSave()
    {
        LoadGameDataFromFile(initialSavePath);
    }

    /// <summary>
    /// Does not handle exceptions
    /// </summary>
    private void SaveGameDataToFile(string filePath)
    {
        FileStream fStream = new FileStream(filePath, FileMode.Create);
        binFormatter.Serialize(fStream, gameData);
        fStream.Close();
        //foreach (var item in roomDataDict)
        //{
        //    Debug.Log($"{item.Key}: {item.Value.objects.Length}");
        //}
    }

    /// <summary>
    /// Does not handle exceptions
    /// </summary>
    private void LoadGameDataFromFile(string filePath)
    {
        FileStream fStream = new FileStream(filePath, FileMode.Open);
        gameData = (GameData)binFormatter.Deserialize(fStream);
        fStream.Close();
    }
}
