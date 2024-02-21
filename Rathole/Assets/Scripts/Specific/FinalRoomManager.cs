using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalRoomManager : MonoBehaviour
{
    [SerializeField] private EndingCutsceneManager.Ending ending = EndingCutsceneManager.Ending.Caught;
    [SerializeField] private float endingCutsceneDelay = 2f;
    [SerializeField] private GameObject door;

    private void Start()
    {
        door.SetActive(false);
        PauseManager.SetManualPauseAllowed(false);
        GameManager.GetPlayer().GetComponent<PlayerHealth>().SetHealthUIVisible(false);
        GameManager.GetPlayer().SetActive(false);
        EndingCutsceneManager.ending = ending;
        FinalScreenManager.foundWeapon = GameManager.GetPlayer().GetComponent<PlayerCombat>().HasWeapon();
        
        IEnumerator waitAndLoadEndingCutsceneScene()
        {
            yield return new WaitForSeconds(endingCutsceneDelay);
            GameManager.LoadEndingCutsceneScene();
        }
        StartCoroutine(waitAndLoadEndingCutsceneScene());

        SoundManager.FadeOutSoundtrack(endingCutsceneDelay);
    }
}
