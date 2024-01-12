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
    private static string saveExtension = ".save";
    //private static string normalSavePath = Application.dataPath + "/TempSaves/saveFile" + saveExtension; // TODO: Replace with persistentDataPath
    //private static string initialSavePath = Application.dataPath + "/PrebakedData/initialSave" + saveExtension;
    private static string normalSavePath = Application.persistentDataPath + "/saveFile" + saveExtension;
    private static string initialSaveResourcePath = "initialSave";
    private static string initialSavePath = Application.dataPath + "/PrebakedData/Resources/" + initialSaveResourcePath + ".bytes";

    private BinaryFormatter binFormatter = new();
    //private Dictionary<string, RoomData> roomDataDict = new();
    private GameData gameData = new();
    private string savePath;

    [Serializable]
    public class RoomData
    {
        //public string name;
        //public bool cleared;
        //public string prefabResourcePath;
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

    public enum SaveFileType
    {
        //New,
        Existing,
        Initial //Only in editor
    }

    public SaveSystem(SaveFileType saveFileType)
    {
        if(saveFileType == SaveFileType.Initial && Application.isPlaying)
        {
            throw new Exception("Can't initialize SaveSystem to work with Initial save file while in play mode");
        }

        savePath = saveFileType == SaveFileType.Initial ? initialSavePath : normalSavePath;
        if (saveFileType == SaveFileType.Existing)
        {
            LoadGameDataFromFile(savePath);
        }
    }

    private SaveSystem() { }

    public void SaveRoomToSystem(GameObject room, string roomName)
    {
        RoomData roomData = new();
        //roomData.prefabResourcePath = roomPrefabResourcePath;
        SavableRoomObject[] savables = room.GetComponentsInChildren<SavableRoomObject>();
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
            throw new Exception("Room with given name does not exist: " + roomName);
        }

        //A lot of path-related exceptions can be thrown here
        GameObject room = GameObject.Instantiate((GameObject)Resources.Load(roomName));

        foreach (SavableRoomObject.RoomObjectData objectData in roomData.objects)
        {
            GameObject roomObject = GameObject.Instantiate((GameObject)Resources.Load(objectData.prefabResourcePath), room.transform);
            roomObject.GetComponent<SavableRoomObject>().LoadData(objectData);
        }

        return room;
    }

    //public void SaveGameDataToDisk()
    //{
    //    SaveGameDataToFile(saveFilePath);
    //}

    //public void SaveGameDataToInitialSave()
    //{
    //    SaveGameDataToFile(initialSavePath);
    //}

    //public void LoadGameDataFromInitialSave()
    //{
    //    LoadGameDataFromFile(initialSavePath);
    //}

    public void SaveToDisk()
    {
        SaveGameDataToFile(savePath);
    }

    //public void LoadFromDisk()
    //{
    //    SaveGameDataToFile(savePath);
    //}

    public static void CreateNewSaveFile()
    {
        SaveSystem saveSystem = new SaveSystem();
        //initialSaveSystem.LoadGameDataFromFile(initialSavePath);
        //saveSystem.gameData = Resources.Load(initialSaveResourcePath);
        //Debug.Log(Resources.Load<TextAsset>(initialSaveResourcePath).bytes);
        saveSystem.gameData = (GameData)saveSystem.binFormatter.Deserialize(new MemoryStream(Resources.Load<TextAsset>(initialSaveResourcePath).bytes));
        
        saveSystem.SaveGameDataToFile(normalSavePath);
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
