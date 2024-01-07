using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    //private static SaveManager sm;
    private static BinaryFormatter binFormatter;
    private static string saveFolderPath = Application.dataPath + "/TempSaves"; // TODO: Replace with persistentDataPath

    //[Serializable]
    //public class RoomObjectData
    //{
    //    //public string name;
    //    public Type type;
    //    public object data;
    //}

    [Serializable]
    public class RoomData
    {
        public string name;
        //public bool cleared;
        public SavableRoomObject.RoomObjectData[] objects;
    }

    public static void SaveRoom(GameObject room, string roomName, string roomPrefabResourcePath)
    {
        //ISavable[] savables = room.GetComponentsInChildren<GameObject>().OfType<ISavable>().ToArray();
        //RoomData roomData = new();
        //roomData.name = "phRoomName"; // PH: Maybe add a room class with field name
        //roomData.objects = new RoomObjectData[savables.Length];

        //for (int i = 0; i < savables.Length; i++)
        //{
        //    roomData.objects[i].type = savables[i].GetType(); //NOTE: I'm not sure if this will serialize
        //    roomData.objects[i].data = savables[i].GetSaveData();
        //}

        //string path = sm.saveFolderPath + "/" + "phRoomSaveName";
        //FileStream file = new FileStream(path, FileMode.Create);
        //sm.binFormatter.Serialize(file, roomData);
        //file.Close();

        throw new Exception("Unimplemented");
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
