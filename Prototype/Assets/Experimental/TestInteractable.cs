using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public override void Interact(PlayerInteractions interactionAgent)
    {
        Destroy(gameObject);
    }
}