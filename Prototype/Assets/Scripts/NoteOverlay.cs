using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteOverlay : MonoBehaviour
{
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private TextMeshProUGUI text;

    private static NoteOverlay no;

    private NoteData currentNote;
    private bool noteWasIsBeingShownThisFrame;

    public static void ShowNote(NoteData note)
    {
        no.currentNote = note;
        no.text.text = no.currentNote.text;
        no.parentGameObject.SetActive(true);
        no.SetNoteWasBeingShownThisFrame();
        PauseManager.SetPaused(true);
    }

    public static void CloseNote()
    {
        no.parentGameObject.SetActive(false);
        no.SetNoteWasBeingShownThisFrame();
        PauseManager.SetPaused(false);
    }

    public static bool NoteIsBeingShown()
    {
        return no.parentGameObject.activeSelf;
    }

    public static bool NoteWasBeingShownThisFrame()
    {
        return no.noteWasIsBeingShownThisFrame;
    }

    private void SetNoteWasBeingShownThisFrame()
    {
        StartCoroutine(SetNoteWasBeingShownThisFrameCoroutine());
    }

    private IEnumerator SetNoteWasBeingShownThisFrameCoroutine()
    {
        yield return new WaitForEndOfFrame();
        noteWasIsBeingShownThisFrame = parentGameObject.activeSelf;
    }

    private void Awake()
    {
        if (no != null)
        {
            Debug.LogError("More than 1 NoteOverlay in the scene");
            return;
        }
        no = this;
    }

    private void Start()
    {
        CloseNote();
    }

    private void Update()
    {
        if(InputManager.GetButtonDown(InputManager.InputButton.Pause) && NoteIsBeingShown())
        {
            CloseNote();
        }
    }
}
