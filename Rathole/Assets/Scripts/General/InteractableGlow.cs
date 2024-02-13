using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InteractableGlow : MonoBehaviour
{
    const float ADDITIONAL_SCALE_MULTIPLIER = 1.1f;

    [SerializeField] private Camera mainCamera;
    [SerializeField][Range(0, 1)] private float defaultMaxGlow = 0.35f;
    [SerializeField] private float pulsePeriod = 1f;

    private SpriteRenderer sprite;

    private float maxGlow;
    private bool pulseEnabled;
    private float pulseStartTime;

    public void StartPulse()
    {
        pulseEnabled = true;
        pulseStartTime = Time.time;
    }

    public void StopPulse()
    {
        pulseEnabled = false;
        Color color = sprite.color;
        color.a = 0f;
        sprite.color = color;
    }

    public void SetMaxGlowBrightness(float maxGlowBrightness)
    {
        maxGlow = maxGlowBrightness;
    }

    public void ResetMaxGlowBrightness()
    {
        maxGlow = defaultMaxGlow;
    }

    public int GetSortingLayerID()
    {
        return sprite.sortingLayerID;
    }

    public int GetOrderInLayer()
    {
        return sprite.sortingOrder;
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        maxGlow = defaultMaxGlow;
    }

    private void Start()
    {
        UpdateScale();
    }

    private void Update()
    {
        if (pulseEnabled)
        {
            UpdateScale();

            Color color = sprite.color;
            color.a = maxGlow * (1 + Mathf.Sin(((Time.time - pulseStartTime) * 2 * Mathf.PI) / pulsePeriod - Mathf.PI / 2)) / 2;
            sprite.color = color;
        }
    }

    private void UpdateScale()
    {
        float y = mainCamera.orthographicSize * 2f * ADDITIONAL_SCALE_MULTIPLIER;
        float x = y * ((float)Screen.width / (float)Screen.height);
        transform.localScale = new Vector3(x, y, 1f);
    }
}
