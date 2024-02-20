using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTotem : Interactable
{
    [SerializeField] private float cooldown = 1f;

    private float lastSaveTime = float.NegativeInfinity;
    public override void Interact(PlayerInteractions interactionAgent)
    {
        if (Time.time < lastSaveTime + cooldown) return;
        interactionAgent.GetComponent<Health>().RestoreAllHealth();
        RuntimeSaveManager.SaveGame();
        ScreenEffectManager.ShowMessage("Progress saved\nYour health has been restored");
        SoundManager.PlaySoundEffect(SoundName.SaveTotem);
        lastSaveTime = Time.time;
    }
}
