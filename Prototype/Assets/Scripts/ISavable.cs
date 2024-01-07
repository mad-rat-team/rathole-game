using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    public object GetState();
    public void LoadState(object state);
}
