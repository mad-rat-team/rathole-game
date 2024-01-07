using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class PrefabBaker
{
    private static string inputFolderPath = Application.dataPath + "/Prefabs/Rooms";
    private static string outputFolderPath = Application.dataPath + "/Prefabs/Resources";
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
        DirectoryInfo dirInfo = new DirectoryInfo(inputFolderPath);
        foreach (FileInfo fileInfo in dirInfo.GetFiles())
        {
            if (fileInfo.Extension != prefabExtension) continue;
            GameObject initialRoomPrefab = PrefabUtility.LoadPrefabContents(fileInfo.FullName);
            if (initialRoomPrefab.tag != roomTag) continue;
            //Debug.Log(fileInfo.Name.TrimEnd(prefabExtension.ToCharArray()));

            string newRoomPrefabPath = outputFolderPath + "/" + fileInfo.Name;
            //SaveManager.SaveRoom(initialRoomPrefab, fileInfo.Name.TrimEnd(prefabExtension.ToCharArray()), newRoomPrefabPath);

            foreach (SavableRoomObject savable in initialRoomPrefab.GetComponentsInChildren<SavableRoomObject>())
            {
                if (savable.gameObject == null) continue;
                GameObject.DestroyImmediate(savable.gameObject);
            }

            PrefabUtility.SaveAsPrefabAsset(initialRoomPrefab, newRoomPrefabPath);
        }
        //GameObject prefabGO = PrefabUtility.LoadPrefabContents(inputFolderPath + "/");
    }
}
