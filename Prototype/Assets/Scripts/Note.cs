using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{
    [SerializeField] private NoteData noteData;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        NoteOverlay.ShowNote(noteData);
    }
}
