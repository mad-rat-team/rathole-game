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
    private bool useUnscaledTime;

    public static void Fade(Color start, Color target, float fadeDuration, float restDuration, bool useUnscaledTime)
    {
        sem.useUnscaledTime = useUnscaledTime;
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
        sem.startTime = (useUnscaledTime ? Time.unscaledTime : Time.time) + restDuration;
    }

    public static void FadeFromCurrent(Color target, float fadeDuration, float restDuration, bool useUnscaledTime)
    {
        Fade(sem.fadeImage.color, target, fadeDuration, restDuration, useUnscaledTime);
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
        //Debug.Log($"{Time.deltaTime} {Time.unscaledDeltaTime}");
        if (isLerping)
        {
            float currentTime = (useUnscaledTime ? Time.unscaledTime : Time.time);
            fadeImage.color = Color.Lerp(startColor, targetColor, (currentTime - startTime) / lerpDuration);
            if (currentTime > startTime + lerpDuration)
            {
                isLerping = false;
            }
        }
    }
}
