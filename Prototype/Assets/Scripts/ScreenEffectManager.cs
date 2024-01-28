using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    private static ScreenEffectManager sem;

    private Color startColor;
    private Color targetColor;
    private float startTime;
    private float lerpDuration;
    private bool isLerping = false;

    public static void Fade(Color start, Color target, float restDuration, float fadeDuration)
    {
        sem.isLerping = true;
        sem.startColor = start;
        sem.targetColor = target;
        if (fadeDuration <= 0)
        {
            fadeDuration = float.Epsilon;
        }
        sem.lerpDuration = fadeDuration;
        if (restDuration < 0)
        {
            restDuration = 0;
        }
        sem.startTime = Time.time + restDuration;
    }

    public static void FadeFromCurrent(Color target, float duration, float restDuration)
    {
        Fade(sem.fadeImage.color, target, duration, restDuration);
    }

    private void Awake()
    {
        if (sem != null)
        {
            Debug.LogWarning("More than 1 ScreenEffectManager in the scene");
            return;
        }
        sem = this;

        startColor = fadeImage.color;
        targetColor = fadeImage.color;
    }

    private void Update()
    {
        if (isLerping)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, (Time.time - startTime) / lerpDuration);
            if (Time.time > startTime + lerpDuration)
            {
                isLerping = false;
            }
        }
    }
}
