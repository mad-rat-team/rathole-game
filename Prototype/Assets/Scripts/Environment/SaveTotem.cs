using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTotem : Interactable
{
    public override void Interact(PlayerInteractions interactionAgent)
    {
        interactionAgent.GetComponent<Health>().RestoreAllHealth();
        RuntimeSaveManager.SaveGame();
        ScreenEffectManager.ShowMessage("Progress saved\nYour health has been restored");
    }
}
