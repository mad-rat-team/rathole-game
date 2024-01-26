using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 0f;

    // NOTE: If NPCs can also interact, PlayerInteractions should be replaced with InteractionAgent abstract class or interface
    public abstract void Interact(PlayerInteractions interactionAgent);
    public float GetInteractionRadius()
    {
        return interactionRadius;
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
