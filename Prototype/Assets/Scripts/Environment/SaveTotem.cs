using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTotem : Interactable
{
    public override void Interact(PlayerInteractions interactionAgent)
    {
        RuntimeSaveManager.SaveGame();
        interactionAgent.GetComponent<Health>().RestoreAllHealth();
    }
}
