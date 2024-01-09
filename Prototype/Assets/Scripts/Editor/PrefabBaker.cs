using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class PrefabBaker
{
    private static string inputFolderPath = Application.dataPath + "/Prefabs/Rooms";
    private static string outputFolderResourcePath = "";
    private static string outputFolderPath =
        Application.dataPath + "/Prefabs/Resources"
        + (outputFolderResourcePath != "" ? ("/" + outputFolderResourcePath) : "");

    private static string prefabExtension = ".prefab";
    private static string roomTag = "Room";

    //public class RoomBakerWindow : EditorWindow
    //{
    //    public static void ShowWindow()
    //    {
    //        EditorWindow.GetWindow(typeof(RoomBakerWindow));
    //    }

    //    private void OnGUI()
    //    {
    //        GUILayout.Label("Test Label", EditorStyles.boldLabel);
    //    }
    //}

    [MenuItem("Custom/Bake Room Prefabs")]
    private static void BakeRoomPrefabs()
    {
        SaveSystem saveSystem = new();
        DirectoryInfo dirInfo = new DirectoryInfo(inputFolderPath);
        foreach (FileInfo fileInfo in dirInfo.GetFiles())
        {
            if (fileInfo.Extension != prefabExtension) continue;
            GameObject roomPrefab = PrefabUtility.LoadPrefabContents(fileInfo.FullName);
            if (roomPrefab.tag != roomTag) continue;

            string roomName = fileInfo.Name.TrimEnd(prefabExtension.ToCharArray());
            string roomResourcePath = (outputFolderResourcePath != "" ? (outputFolderResourcePath + "/") : "") + roomName;

            //saveSystem.SaveRoomToSystem(roomPrefab, fileInfo.Name.TrimEnd(prefabExtension.ToCharArray()), newRoomPrefabPath);
            saveSystem.SaveRoomToSystem(roomPrefab, roomResourcePath, roomResourcePath);

            foreach (SavableRoomObject savable in SavableRoomObject.GetSavableRoomObjects(roomPrefab))
            {
                GameObject.DestroyImmediate(savable.gameObject);
            }

            string newRoomPrefabPath = outputFolderPath + "/" + fileInfo.Name;
            PrefabUtility.SaveAsPrefabAsset(roomPrefab, newRoomPrefabPath);
        }
        saveSystem.SaveGameDataToInitialSave();
    }
}
