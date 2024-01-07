using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// GameObject should have this component if some changes made to it on runtime should be stored when leaving the room
/// </summary>
public class SavableRoomObject : MonoBehaviour
{
    [SerializeField] private string prefabResourcePath;

    //public string GetObjectType() {
    //    return GetType().ToString();
    //}

    public class RoomObjectData
    {
        public string prefabResourcePath;
        public object state;
    }

    public RoomObjectData GetData()
    {
        RoomObjectData data = new();
        data.prefabResourcePath = prefabResourcePath;
        data.state = getSavableComponent().GetState();

        return data;
    }
    public void LoadState(object state)
    {
        getSavableComponent().LoadState(state);
    }

    /// <exception cref="Exception">Throws an exception if there are more or less than 1 component that implement ISavable</exception>
    private ISavable getSavableComponent()
    {
        ISavable[] savableComponents = GetComponents<Component>().OfType<ISavable>().ToArray();
        if (savableComponents.Length == 0)
        {
            throw new Exception("No component that implements ISavable was found on object with Component SavableRoomObject.");
        }
        if (savableComponents.Length > 1)
        {
            throw new Exception("Object should not have more than 1 component that implements ISavable.");
        }

        return savableComponents[0];
    }
}
