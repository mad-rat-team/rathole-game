using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalRoomManager : MonoBehaviour
{
    [SerializeField] private EndingCutsceneManager.Ending ending = EndingCutsceneManager.Ending.Caught;

    private void Start()
    {
        EndingCutsceneManager.ending = ending;
        GameManager.LoadEndingCutsceneScene();
    }
}
