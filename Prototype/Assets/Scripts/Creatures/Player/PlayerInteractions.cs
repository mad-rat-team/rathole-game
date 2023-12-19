using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 0.75f;

    public float GetInteractionRadiusSquared()
    {
        return interactionRadius * interactionRadius;
    }

    [SerializeField] private bool drawDebugGizmos = false;
    private void OnDrawGizmosSelected()
    {
        if (!drawDebugGizmos) return;
        Gizmos.color = Color.white;
        int resolution = 45;
        Vector2 prevPoint = new Vector2(Mathf.Cos(((float)resolution - 1)/resolution * 2 * Mathf.PI), Mathf.Sin(((float)resolution - 1)/resolution * 2 * Mathf.PI)/2) * interactionRadius;
        for (float i = 0; i < resolution; i++)
        {
            Vector2 newPoint = new Vector2(Mathf.Cos(i/resolution * 2 * Mathf.PI), Mathf.Sin(i/resolution * 2 * Mathf.PI)/2) * interactionRadius;
            Gizmos.DrawLine((Vector3)prevPoint + transform.position, (Vector3)newPoint + transform.position);
            prevPoint = newPoint;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            InteractionManager.Interact(this);
        }   
    }
}