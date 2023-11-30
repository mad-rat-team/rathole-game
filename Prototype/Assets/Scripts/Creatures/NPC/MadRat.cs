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
        health.OnKnockbackReceived += movement.StartKnockback;
        //health.OnKnockbackEnded += HandleKnockbackEnded;
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
        }
        else
        {
            movement.SetTarget(transform.position);
        }

        GetComponentInChildren<SpriteRenderer>().flipX = movement.GetMoveDir().x > 0; //PH
    }
}
