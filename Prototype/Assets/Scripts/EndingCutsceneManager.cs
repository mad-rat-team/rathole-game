using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCutsceneManager : MonoBehaviour
{
    public void Start()
    {
        Time.timeScale = 1f;
        ScreenEffectManager.Fade(Color.black, new Color(0, 0, 0, 0), 1.5f, 0.5f, false);

        IEnumerator waitAndGoToFinalScreen()
        {
            yield return new WaitForSeconds(3f);
            GameManager.LoadFinalScreenScene();
        }

        IEnumerator waitAndFade()
        {
            yield return new WaitForSeconds(3f);
            ScreenEffectManager.FadeFromCurrent(Color.black, 2f, 0f, false);
            StartCoroutine(waitAndGoToFinalScreen());
        }

        StartCoroutine(waitAndFade());
    }
}
