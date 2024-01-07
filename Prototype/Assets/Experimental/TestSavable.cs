using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSavable : MonoBehaviour, ISavable
{
    [SerializeField] private string someData;

    public object GetState()
    {
        return someData;
    }

    public void LoadState(object state)
    {
        someData = (string)state;
    }
}
