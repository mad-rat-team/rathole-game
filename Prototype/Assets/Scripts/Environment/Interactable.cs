using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 0f;

    [Header("Pulse")]
    [SerializeField] private bool pulseWhenInteractable = true;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField][Range(0, 1)] private float maxWhitening = 1f;
    [SerializeField] private float pulsePeriod = 0.5f;

    private bool isClosestInteractable = true;
    private float pulseStartTime;
    private Color startColor;

    // NOTE: If NPCs can also interact, PlayerInteractions should be replaced with InteractionAgent abstract class or interface
    public abstract void Interact(PlayerInteractions interactionAgent);
    public float GetInteractionRadius()
    {
        return interactionRadius;
    }

    public void SetIsClosestInteractable(bool newIsClosestInteractable)
    {
        if (!newIsClosestInteractable)
        {
            isClosestInteractable = false;
            sprite.color = startColor;
            return;
        }

        if (isClosestInteractable) return;

        isClosestInteractable = true;
        pulseStartTime = Time.time;
    }

    private void Start()
    {
        if (pulseWhenInteractable)
        {
            startColor = sprite.color;
        }
    }

    private void Update()
    {
        if(isClosestInteractable && pulseWhenInteractable)
        {
            //float whitening = maxWhitening * Mathf.Sin(((Time.time - pulseStartTime) * 2 * Mathf.PI) / pulsePeriod);
            float whitening = maxWhitening * (1 + Mathf.Sin(((Time.time - pulseStartTime) * 2 * Mathf.PI) / pulsePeriod - Mathf.PI / 2)) / 2;
            Debug.Log(new Color(startColor.r + whitening, startColor.g + whitening, startColor.b + whitening, sprite.color.a));
            //Debug.Log($"{name}: {startColor}    {sprite}");
            sprite.color = new Color(startColor.r + whitening, startColor.g + whitening, startColor.b + whitening, sprite.color.a);
        }
    }

    private void OnDestroy()
    {
        InteractionManager.RemoveInteractable(this);
    }

    //Debug
    [SerializeField] private bool drawDebugGizmos = false;

    private void OnDrawGizmosSelected()
    {
        if (!drawDebugGizmos) return;
        Gizmos.color = Color.white;
        int resolution = 45;
        Vector2 prevPoint = new Vector2(Mathf.Cos(((float)resolution - 1) / resolution * 2 * Mathf.PI), Mathf.Sin(((float)resolution - 1) / resolution * 2 * Mathf.PI) / 2) * interactionRadius;
        for (float i = 0; i < resolution; i++)
        {
            Vector2 newPoint = new Vector2(Mathf.Cos(i / resolution * 2 * Mathf.PI), Mathf.Sin(i / resolution * 2 * Mathf.PI) / 2) * interactionRadius;
            Gizmos.DrawLine((Vector3)prevPoint + transform.position, (Vector3)newPoint + transform.position);
            prevPoint = newPoint;
        }
    }
}
