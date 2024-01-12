using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private static InteractionManager im;
    private List<Interactable> interactables = new List<Interactable>();

    /// <summary>
    /// Returns the closest of all interactables within interactionAgent's interaction radius. If there are no interactables within radius, returns NULL.
    /// </summary>
    /// <returns>May be NULL</returns>
    public static Interactable GetPotentialInteractable(PlayerInteractions interactionAgent)
    {
        Vector2 agentPos = interactionAgent.transform.position;

        List<Interactable> interactablesWithinRadius = new List<Interactable>();
        foreach (var interactable in im.interactables)
        {
            if (Shortcuts.IsoToReal(agentPos - (Vector2)interactable.transform.position).SqrMagnitude() <= interactionAgent.GetInteractionRadiusSquared())
            {
                interactablesWithinRadius.Add(interactable);
            }
        }

        if(interactablesWithinRadius.Count == 0)
        {
            return null;
        }

        interactablesWithinRadius.Sort((a, b) =>
        {
            float sqrDistToA = Shortcuts.IsoToReal((Vector2)a.transform.position - agentPos).SqrMagnitude();
            float sqrDistToB = Shortcuts.IsoToReal((Vector2)b.transform.position - agentPos).SqrMagnitude();

            if (sqrDistToA > sqrDistToB)
            {
                return 1;
            }
            else if (sqrDistToA < sqrDistToB)
            {
                return -1;
            }

            return 0;
        }); // List.Sort() sorts in ascending order, so the comarator returns a > b if a is further away from agentPos than b

        return interactablesWithinRadius[0];
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

    private void Start()
    {
        if(im != null)
        {
            Debug.LogWarning("More than 1 InteractionManager in the scene");
        }

        im = this;
        UpdateInteractables();

        RoomManager.OnRoomChanged += UpdateInteractables;
    }

    private void UpdateInteractables()
    {
        interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None).ToList();
    }
}
