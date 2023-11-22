using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(NPCHealth))]
public class MadRat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NPCMovement movement;
    private NPCHealth health;

    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        health = GetComponent<NPCHealth>();

        //Subscribing to events - PH, should be abstracted to not write this every time for a new enemy
        health.OnKnockbackReceived += HandleKnockbackReceived;
        health.OnKnockbackEnded += HandleKnockbackEnded;
    }

    private void Start()
    {
        movement.SetIsMoving(true);
    }

    private void Update()
    {
        if(Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 7*7)
        {
            movement.SetTarget(player.transform.position);
            //Debug.Log("aa");
        }
        else
        {
            movement.SetTarget(transform.position);
        }

        GetComponentInChildren<SpriteRenderer>().flipX = movement.GetCurrentMoveDir().x > 0; //PH
    }

    private void HandleKnockbackReceived(Vector2 origin, float knockbackForce)
    {
        movement.SetIsMoving(false);
        movement.Push(Shortcuts.NormalizeIso((Vector2)transform.position - origin) * knockbackForce); //NOTE: Maybe using Unity's physics for such things is a bad idea since our game is isometric
    }

    private void HandleKnockbackEnded()
    {
        movement.SetIsMoving(true);
    }
}
