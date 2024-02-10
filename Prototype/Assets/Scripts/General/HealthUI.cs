using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class HealthUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image image;

    public void UpdateHeartCount(int healthAmount)
    {
        Vector2 newSizeDelta = rectTransform.sizeDelta;
        newSizeDelta.x = image.sprite.texture.width * Mathf.Clamp(healthAmount, 0, int.MaxValue);
        rectTransform.sizeDelta = newSizeDelta;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
}
