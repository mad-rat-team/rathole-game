using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private InteractableGlow interactableGlow;

    private static InteractionManager im;

    private PlayerInteractions playerInteractionAgent;

    private List<Interactable> interactables = new List<Interactable>();
    private Interactable closestInteractable = null;

    /// <summary>
    /// Returns the closest of all interactables within interactionAgent's interaction radius.
    /// If there are no interactables within radius, returns NULL.
    /// </summary>
    /// <returns>May be NULL</returns>
    public static Interactable GetPotentialInteractable(PlayerInteractions interactionAgent)
    {
        Vector2 agentPos = interactionAgent.transform.position;

        Interactable closest = null;
        float minDistanceToPlayerSquared = float.PositiveInfinity;

        foreach (Interactable interactable in im.interactables)
        {
            float distanceSquared = Shortcuts.IsoToReal(agentPos - (Vector2)interactable.transform.position).SqrMagnitude();
            float maxAllowedDistance = interactionAgent.GetInteractionRadius() + interactable.GetInteractionRadius();

            if (distanceSquared < minDistanceToPlayerSquared && distanceSquared <= maxAllowedDistance * maxAllowedDistance)
            {
                closest = interactable;
                minDistanceToPlayerSquared = distanceSquared;
            }
        }

        return closest;
    }

    public static void Interact(PlayerInteractions interactionAgent)
    {
        Interactable interactable = GetPotentialInteractable(interactionAgent);
        if(interactable != null)
        {
            interactable.Interact(interactionAgent);
        }
    }

    /// <summary>
    /// Should be called when destroying gameobjects that are interactables
    /// </summary>
    public static void RemoveInteractable(Interactable interactable)
    {
        im.interactables.Remove(interactable);
    }

    private void Awake()
    {
        if (im != null)
        {
            Debug.LogWarning("More than 1 InteractionManager in the scene");
            return;
        }

        im = this;
    }

    private void Start()
    {
        playerInteractionAgent = GameManager.GetPlayer().GetComponent<PlayerInteractions>();
        UpdateInteractables();
        RoomManager.OnRoomChanged += UpdateInteractables;
    }

    private void Update()
    {
        Interactable prevClosestInteractable = closestInteractable;
        closestInteractable = GetPotentialInteractable(playerInteractionAgent);
        if (closestInteractable != prevClosestInteractable)
        {
            if (prevClosestInteractable != null)
            {
                prevClosestInteractable.SetGlowMaskActive(false);
            }

            if (closestInteractable != null)
            {
                closestInteractable.SetGlowMaskActive(true);
                interactableGlow.StartPulse();
            }
            else
            {
                interactableGlow.StopPulse();
            }
        }
    }

    private void UpdateInteractables()
    {
        interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None).ToList();
    }
}
