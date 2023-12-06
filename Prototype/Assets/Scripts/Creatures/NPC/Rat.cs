using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(Health))]
public class Rat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NPCMovement movement;
    private Health health;

    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
        health = GetComponent<Health>();

        //Subscribing to events - PH, should be abstracted to not write this every time for a new enemy
        health.OnKnockbackReceived += movement.StartKnockback;
        //health.OnKnockbackEnded += HandleKnockbackEnded;
    }

    //public bool isMoving = false; //TEST
    //private void Start()
    //{
    //    movement.SetIsMoving(isMoving);
    //}

    private void Update()
    {
        if (Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 7 * 7)
        {
            movement.SetTarget(player.transform.position);
        }
        else
        {
            movement.SetTarget(transform.position);
        }

        GetComponentInChildren<Animator>().SetFloat("WalkDir", ((Vector2.SignedAngle(Vector2.right, movement.GetMoveDir())+360f)%360f)/360f);
    }
}
