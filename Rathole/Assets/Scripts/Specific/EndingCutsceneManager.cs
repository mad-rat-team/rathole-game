using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingCutsceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public enum Ending
    {
        Caught = 1,
        Escape = 2
    }

    public static Ending ending;

    private void Start()
    {
        Time.timeScale = 1f; //just in case

        text.text = GetEndingText();

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

    private string GetEndingText()
    {
        return ending switch
        {
            Ending.Caught => "\"What are you doing here, Smirnov?\"",
            Ending.Escape => "You made it to the surface",
            _ => ":)"
        };
    }
}
