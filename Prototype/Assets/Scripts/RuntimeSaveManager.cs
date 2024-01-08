using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeSaveManager : MonoBehaviour
{
    //private static RuntimeSaveManager rsm;

    private static SaveSystem saveSystem;
    public static GameObject LoadRoom(string roomName)
    {
        LazyInit();
        return RuntimeSaveManager.saveSystem.LoadRoom(roomName);
    }

    private static void LazyInit()
    {
        if (saveSystem != null) return;
        saveSystem = new();
        saveSystem.LoadGameDataFromInitialSave();
    }

    //private void Awake() //Should be awake instead of start since other scripts depend on this being initialized
    //{
    //    if(rsm != null)
    //    {
    //        Debug.LogWarning("More than 1 RuntimeSaveManager in the scene");
    //    }

    //    saveSystem.LoadGameDataFromInitialSave();
    //}
}
