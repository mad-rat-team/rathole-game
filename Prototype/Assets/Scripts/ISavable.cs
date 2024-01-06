using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    //public string GetObjectType() {
    //    return GetType().ToString();
    //}
    public object GetSaveData();
    public void LoadSaveData(object saveData);
}
