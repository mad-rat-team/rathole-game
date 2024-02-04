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
    private int currentPage;

    private bool noteWasIsBeingShownThisFrame;

    public static void ShowNote(NoteData note)
    {
        no.currentNote = note;
        if (!no.ShowPage(0)) throw new System.Exception("Note is empty");
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

    public static void ShowNextPage()
    {
        no.ShowPage(no.currentPage + 1);
    }

    public static void ShowPreviousPage()
    {
        no.ShowPage(no.currentPage - 1);
    }

    public static bool NoteIsBeingShown()
    {
        return no.parentGameObject.activeSelf;
    }

    public static bool NoteWasBeingShownThisFrame()
    {
        return no.noteWasIsBeingShownThisFrame;
    }

    /// <returns>Returns <b>false</b> if a page with a given number does not exist. Othervise returns <b>true</b></returns>
    private bool ShowPage(int pageNumber)
    {
        if (currentNote.pages.Length <= pageNumber || pageNumber < 0) return false;
        text.text = currentNote.pages[pageNumber];
        currentPage = pageNumber;
        return true;
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
