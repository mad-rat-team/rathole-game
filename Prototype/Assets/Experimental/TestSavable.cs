using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSavable : Interactable/*, ISavable*/
{
    //[System.Serializable] private class State
    //{
    //    public SerializableVector3 position;
    //    public string someData;
    //}

    //[SerializeField] private string someData;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        Destroy(this.gameObject);
    }

    //public object GetState()
    //{
    //    State state = new();
    //    state.position = new SerializableVector3(transform.position);
    //    state.someData = someData;
    //    return state;
    //}

    //public void LoadState(object stateObj)
    //{
    //    State state = (State)stateObj;
    //    someData = state.someData;
    //    transform.position = state.position.GetVector3();
    //}
}
