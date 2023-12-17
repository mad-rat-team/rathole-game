using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // NOTE: If NPCs can also interact, PlayerInteractions should be replaced with InteractionAgent abstract class or interface
    public abstract void Interact(PlayerInteractions interactionAgent);
}
