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

    [Serializable]
    public class SavableComponentData
    {
        public Type type;
        public object state;
    }

    [Serializable]
    public class RoomObjectData
    {
        public string prefabResourcePath;
        //public object state;
        public SavableComponentData[] savableComponents;
    }

    public RoomObjectData GetData()
    {
        RoomObjectData data = new();
        data.prefabResourcePath = prefabResourcePath;
        //data.state = getSavableComponent().GetState();
        ISavable[] savableComponents = getSavableComponents();
        data.savableComponents = new SavableComponentData[savableComponents.Length];
        for (int i = 0; i < savableComponents.Length; i++)
        {
            data.savableComponents[i] = new SavableComponentData();
            data.savableComponents[i].type = savableComponents[i].GetType();
            data.savableComponents[i].state = savableComponents[i].GetState();
        }

        return data;
    }

    public void LoadData(RoomObjectData data)
    {
        //getSavableComponent().LoadState(state);
        foreach (SavableComponentData componentData in data.savableComponents)
        {
            Component component;
            bool success = TryGetComponent(componentData.type, out component);
            if (!success)
            {
                throw new Exception($"Gameobject {gameObject.name} does not have component {componentData.type}");
            }

            ISavable savable = (ISavable)component;
            savable.LoadState(componentData.state);
        }
    }

    private ISavable[] getSavableComponents()
    {
        return GetComponents<Component>().OfType<ISavable>().ToArray();
    }
}
