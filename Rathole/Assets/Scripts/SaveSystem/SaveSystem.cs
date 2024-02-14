using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private static string saveExtension = ".save";
    private static string normalSavePath = Application.persistentDataPath + "/saveFile" + saveExtension;
    private static string initialSaveResourcePath = "initialSave";
    private static string initialSavePath = Application.dataPath + "/PrebakedData/Resources/" + initialSaveResourcePath + ".bytes";

    private BinaryFormatter binFormatter = new();
    private GameData gameData = new();
    private string savePath;

    [Serializable]
    public class RoomData
    {
        //public bool cleared;
        public SavableRoomObject.RoomObjectData[] objects;
    }

    [Serializable]
    public class GameData
    {
        //public string currentRoomName;
        //public SerializableVector3 playerPosition;
        //public object playerInventoryState;

        public object playerState;
        public Dictionary<string, RoomData> roomDataDict;

        public GameData()
        {
            roomDataDict = new();
        }
    }

    public enum SaveFileType
    {
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
            LoadFromDisk();
        }
    }

    private SaveSystem() { }

    public void SaveRoomToSystem(GameObject room, string roomName)
    {
        RoomData roomData = new();
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

    public void SavePlayerState(object playerState)
    {
        gameData.playerState = playerState;
    }

    public object GetPlayerState()
    {
        return gameData.playerState;
    }

    public void SaveToDisk()
    {
        SaveGameDataToFile(savePath);
    }

    public void LoadFromDisk()
    {
        LoadGameDataFromFile(savePath);
    }

    public static void CreateNewSaveFile()
    {
        SaveSystem saveSystem = new SaveSystem();
        saveSystem.gameData = (GameData)saveSystem.binFormatter.Deserialize(new MemoryStream(Resources.Load<TextAsset>(initialSaveResourcePath).bytes));
        saveSystem.SaveGameDataToFile(normalSavePath);
    }

    public static bool SaveFileExists()
    {
        FileInfo fileInfo = new FileInfo(normalSavePath);
        return fileInfo.Exists;
    }

    /// <summary>
    /// Does not handle exceptions
    /// </summary>
    private void SaveGameDataToFile(string filePath)
    {
        FileStream fStream = new FileStream(filePath, FileMode.Create);
        binFormatter.Serialize(fStream, gameData);
        fStream.Close();
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
