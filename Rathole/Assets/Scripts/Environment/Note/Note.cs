using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{
    [Header("Note")]
    [SerializeField] private NoteData noteData;

    public override void Interact(PlayerInteractions interactionAgent)
    {
        NoteOverlay.ShowNote(noteData);
    }
}
