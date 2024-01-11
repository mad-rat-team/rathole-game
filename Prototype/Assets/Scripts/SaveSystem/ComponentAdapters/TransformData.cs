using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent isn't necessary since every GameObject has a TransformComponent

public class TransformData : MonoBehaviour, ISavable
{
    [System.Serializable]
    private class SaveData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        public SerializableVector3 scale;
    }

    public object GetState()
    {
        SaveData saveData = new();
        saveData.position = new SerializableVector3(transform.position);
        saveData.rotation = new SerializableVector3(transform.rotation.eulerAngles);
        saveData.scale = new SerializableVector3(transform.localScale);

        return saveData;
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        transform.position = saveData.position.GetVector3();
        transform.rotation = Quaternion.Euler(saveData.rotation.GetVector3());
        transform.localScale = saveData.scale.GetVector3();
    }
}
