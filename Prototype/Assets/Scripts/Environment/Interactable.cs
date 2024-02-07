using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 0f;
    [SerializeField] private bool glowsWhenIsClosestInteractable = true;
    [SerializeField] private SpriteRenderer mainSprite;

    private SpriteMask glowMask;

    // NOTE: If NPCs can also interact, PlayerInteractions should be replaced with InteractionAgent abstract class or interface
    public abstract void Interact(PlayerInteractions interactionAgent);
    public float GetInteractionRadius()
    {
        return interactionRadius;
    }

    public void SetGlowMaskActive(bool enabled)
    {
        if (!glowsWhenIsClosestInteractable) return;
        glowMask.enabled = enabled;
    }
    
    public void UpdateGlowMaskTransform()
    {
        if (!glowsWhenIsClosestInteractable) return;
        glowMask.gameObject.transform.position = mainSprite.gameObject.transform.position;
        glowMask.gameObject.transform.localRotation = mainSprite.gameObject.transform.localRotation;
        glowMask.gameObject.transform.localScale = mainSprite.gameObject.transform.localScale;
    }

    private void Awake()
    {
        if (glowsWhenIsClosestInteractable)
        {
            //GameObject glowMaskGO = Instantiate(new GameObject("Glow Mask"), transform);
            GameObject glowMaskGO = new GameObject("Glow Mask");
            glowMaskGO.transform.parent = transform;
            glowMask = glowMaskGO.AddComponent<SpriteMask>();
            glowMask.sprite = mainSprite.sprite;
            glowMask.enabled = false;
            UpdateGlowMaskTransform();
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
