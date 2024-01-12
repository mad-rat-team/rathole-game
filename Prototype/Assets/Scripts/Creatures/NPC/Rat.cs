using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
public class Rat : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float jumpTime = 1f;

    private Movement movement;
    private Health health;

    private GameObject player;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();

        //Subscribing to events - PH, should be abstracted to not write this every time for a new enemy
        health.OnKnockbackReceived += movement.StartKnockback;
    }

    private void Start()
    {
        movement.SetMoveSpeed(walkSpeed);
        player = GameObject.FindWithTag("Player"); //PH
    }

    private void Update()
    {
        if (Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 3 * 3)
        {
            movement.StartJump(player.transform.position, jumpTime);
        }
        else if (Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 7 * 7)
        {
            movement.SetTargetMoveDir(Shortcuts.NormalizeIso(player.transform.position - transform.position));
        }
        else
        {
            movement.SetTargetMoveDir(Vector2.zero);
        }

        //GetComponentInChildren<Animator>().SetFloat("WalkDir", ((Vector2.SignedAngle(Vector2.right, movement.GetWalkDir()) + 360f) % 360f) / 360f); // Not PH
        GetComponentInChildren<Animator>().SetFloat("WalkDir", ((Vector2.SignedAngle(Vector2.right, movement.GetMoveDir()) + 360f) % 360f) / 360f); //PH
    }
}