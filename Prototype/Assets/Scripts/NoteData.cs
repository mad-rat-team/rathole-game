using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NoteData", menuName = "Custom/NoteData")]
public class NoteData : ScriptableObject
{
    [TextArea] public string[] pages;
}
