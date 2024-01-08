using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private static string saveFolderPath = Application.dataPath + "/TempSaves"; // TODO: Replace with persistentDataPath

    private BinaryFormatter binFormatter = new();
    private Dictionary<string, RoomData> serializedRoomsData = new();

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
        public RoomData[] roomsData;
    }

    public void SerializeRoom(GameObject room, string roomName, string roomPrefabResourcePath)
    {
        RoomData roomData = new();
        roomData.prefabResourcePath = roomPrefabResourcePath;
        SavableRoomObject[] savables = SavableRoomObject.GetSavableRoomObjects(room);
        roomData.objects = new SavableRoomObject.RoomObjectData[savables.Length];
        for (int i = 0; i < savables.Length; i++)
        {
            roomData.objects[i] = savables[i].GetData();
        }

        serializedRoomsData[roomName] = roomData;
    }

    public void SaveRoomDataToFile() // PH: Should save to different save files instead of just 1
    {
        FileStream fStream = new FileStream(saveFolderPath + "/" + "testSaveFile.save", FileMode.Create);
        binFormatter.Serialize(fStream, serializedRoomsData);
        fStream.Close();

        //foreach (var item in serializedRoomsData)
        //{
        //    Debug.Log($"{item.Key}: {item.Value.objects.Length}");
        //}
    }

    public void LoadRoomDataFromFile()
    {
        FileStream fStream = new FileStream(saveFolderPath + "/" + "testSaveFile.save", FileMode.Open);
        serializedRoomsData = (Dictionary<string, RoomData>)binFormatter.Deserialize(fStream);
        fStream.Close();
    }

    //private void Start()
    //{
    //    if (sm != null)
    //    {
    //        Debug.LogWarning("More than 1 SaveManager in the scene");
    //    }
    //    sm = this;

    //    //    //test
    //    //    RoomData testRoomData = new RoomData();
    //    //    testRoomData.name = "testRoomName";
    //    //    RoomObjectData testObjectData1 = new();
    //    //    testObjectData1.name = "testObj1";
    //    //    testObjectData1.type = "testType";
    //    //    testObjectData1.state = 3;
    //    //    //testObjectData1.health = 3;

    //    //    RoomObjectData testObjectData2 = new();
    //    //    testObjectData2.name = "testObj2";
    //    //    testObjectData2.type = "testType";
    //    //    testObjectData2.state = "qwerty";
    //    //    //testObjectData2.note = "qwerty";

    //    //    testRoomData.objects = new RoomObjectData[] { testObjectData1, testObjectData2 };

    //    //    string testJson = JsonUtility.ToJson(testRoomData);
    //    //    Debug.Log(testJson);

    //    //    //System.Runtime.Serialization.Json.DataContractJsonSerializer jsonSerializer = new(typeof(RoomObjectData));
    //    //    //BinaryFormatter

    //    //    //BinaryWriter bw = new BinaryWriter();

    //    //    //Debug.Log(Application.persistentDataPath);
    //    //    BinaryFormatter bf = new BinaryFormatter();
    //    //    string path = Application.dataPath + "/testSaveFile.bin";
    //    //    FileStream fStream = new FileStream(path, FileMode.Create);
    //    //    bf.Serialize(fStream, testRoomData);
    //    //    fStream.Close();

    //    //    fStream = new FileStream(path, FileMode.Open);
    //    //    RoomData copy = (RoomData)bf.Deserialize(fStream);
    //    //    fStream.Close();


    //    //    Debug.Log(testRoomData.objects[1].state);
    //    //    Debug.Log(copy.objects[1].state);
    //    //    testRoomData.objects[1].state = 5;
    //    //    Debug.Log(testRoomData.objects[1].state);
    //    //    Debug.Log(copy.objects[1].state);
    //}
}
