using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteOverlay : MonoBehaviour
{
    [SerializeField] private GameObject parentGameObject;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button previousPageButton;
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
        SoundManager.PlaySoundEffect(SoundName.NoteOpen);
    }

    public static void CloseNote()
    {
        no.parentGameObject.SetActive(false);
        no.SetNoteWasBeingShownThisFrame();
        PauseManager.SetPaused(false);
        SoundManager.PlaySoundEffect(SoundName.NoteClose);
    }

    public static void ShowNextPage()
    {
        no.ShowPage(no.currentPage + 1);
        SoundManager.PlaySoundEffect(SoundName.PageTurn);
    }

    public static void ShowPreviousPage()
    {
        no.ShowPage(no.currentPage - 1);
        SoundManager.PlaySoundEffect(SoundName.PageTurn);
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

        nextPageButton.gameObject.SetActive(pageNumber < currentNote.pages.Length - 1);
        previousPageButton.gameObject.SetActive(pageNumber > 0);

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

    private void Update()
    {
        if(InputManager.GetButtonDown(InputManager.InputButton.Pause) && NoteIsBeingShown())
        {
            CloseNote();
        }
    }
}
