using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public static class PrefabBaker
{
    private static string startingRoomName = "StartRoom";
    //private static string startingRoomName = "Room1";

    private static string playerStartPosTag = "PlayerStartPos";
    private static string inputFolderPath = Application.dataPath + "/Prefabs/Rooms";
    //private static string outputFolderResourcePath = "";
    private static string outputFolderPath = Application.dataPath + "/Prefabs/Rooms/Resources"; // Should be a Resources folder

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
        SaveSystem saveSystem = new SaveSystem(SaveSystem.SaveFileType.Initial);
        DirectoryInfo dirInfo = new DirectoryInfo(inputFolderPath);
        bool foundStartingRoom = false;
        foreach (FileInfo fileInfo in dirInfo.GetFiles())
        {
            if (fileInfo.Extension != prefabExtension) continue;

            GameObject roomPrefab = PrefabUtility.LoadPrefabContents(fileInfo.FullName);
            if (roomPrefab.tag != roomTag) continue;

            string roomName = fileInfo.Name.TrimEnd(prefabExtension.ToCharArray());
            saveSystem.SaveRoomToSystem(roomPrefab, roomName);

            foreach (SavableRoomObject savable in GetSavableRoomObjects(roomPrefab))
            {
                GameObject.DestroyImmediate(savable.gameObject);
            }

            // Assigning starting room and player position
            if (!foundStartingRoom && roomName == startingRoomName)
            {
                foundStartingRoom = true;
                Vector2 playerPos = Vector2.zero;
                bool foundPlayerStartPos = false;
                foreach(Transform child in roomPrefab.transform)
                {
                    if (child.tag == playerStartPosTag)
                    {
                        foundPlayerStartPos = true;
                        playerPos = child.position;
                        break;
                    }
                }
                //saveSystem.SavePlayerData(roomName, playerPos, Inventory.GetEmptyState());
                saveSystem.SavePlayerState(new PlayerData.State
                {
                    position = new SerializableVector3(playerPos),
                    currentRoomName = roomName,
                    inventoryState = Inventory.GetEmptyState(),
                    health = 1, // Maybe this value should be stored somewhere, idk
                });

                if (!foundPlayerStartPos)
                {
                    Debug.LogWarning("Starting room has no GameObject with tag \"PlayerStartPos\"");
                }
            }

            string newRoomPrefabPath = outputFolderPath + "/" + fileInfo.Name;
            PrefabUtility.SaveAsPrefabAsset(roomPrefab, newRoomPrefabPath);

            PrefabUtility.UnloadPrefabContents(roomPrefab);
        }
        if (!foundStartingRoom)
        {
            Debug.LogWarning($"No room with name \"{startingRoomName}\" was found. Starting room was not assigned in the save file.");
        }

        saveSystem.SaveToDisk();
        AssetDatabase.Refresh();
    }

    private static SavableRoomObject[] GetSavableRoomObjects(GameObject room)
    {
        return room.GetComponentsInChildren<SavableRoomObject>().Where(
            savable =>
            {
                if (savable.gameObject == null)
                {
                    Debug.LogError("One GameObject cannot have multiple SavableRoomObject components.");
                    return false;
                }
                return true;
            }).ToArray();
    }
}
