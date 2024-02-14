using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI messageText;

    private static ScreenEffectManager sem;

    private Color fadeStartColor;
    private Color fadeTargetColor;
    private float fadeStartTime;
    private float fadeDuration;
    private bool isFading = false;
    private bool fadeUseUnscaledTime;

    private Animator messageAnimator;

    public static void Fade(Color start, Color target, float fadeDuration, float restDuration, bool useUnscaledTime)
    {
        sem.fadeUseUnscaledTime = useUnscaledTime;
        sem.isFading = true;
        sem.fadeStartColor = start;
        sem.fadeTargetColor = target;
        if (fadeDuration <= 0)
        {
            fadeDuration = float.Epsilon;
        }
        sem.fadeDuration = fadeDuration;
        if (restDuration < 0)
        {
            restDuration = 0;
        }
        sem.fadeStartTime = (useUnscaledTime ? Time.unscaledTime : Time.time) + restDuration;
    }

    public static void FadeFromCurrent(Color target, float fadeDuration, float restDuration, bool useUnscaledTime)
    {
        Fade(sem.fadeImage.color, target, fadeDuration, restDuration, useUnscaledTime);
    }

    public static void ShowMessage(string text)
    {
        sem.messageText.text = text;
        sem.messageAnimator.SetTrigger("ShowMessage");
    }

    public static void SetMessageVisible(bool visible)
    {
        sem.messageText.enabled = visible;
    }

    private void Awake()
    {
        if (sem != null)
        {
            Debug.LogWarning("More than 1 ScreenEffectManager in the scene");
            return;
        }
        sem = this;

        fadeStartColor = fadeImage.color;
        fadeTargetColor = fadeImage.color;
    }

    private void Start()
    {
        messageAnimator = messageText?.GetComponent<Animator>();
    }

    private void Update()
    {
        //Debug.Log($"{Time.deltaTime} {Time.unscaledDeltaTime}");
        if (isFading)
        {
            float currentTime = (fadeUseUnscaledTime ? Time.unscaledTime : Time.time);
            fadeImage.color = Color.Lerp(fadeStartColor, fadeTargetColor, (currentTime - fadeStartTime) / fadeDuration);
            if (currentTime > fadeStartTime + fadeDuration)
            {
                isFading = false;
            }
        }
    }
}
