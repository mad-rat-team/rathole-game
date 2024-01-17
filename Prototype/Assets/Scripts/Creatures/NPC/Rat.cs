using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Attacker))]
public class Rat : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float jumpTime = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D attackColl;

    private Movement movement;
    private Health health;
    private Attacker attacker;

    private GameObject player;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        health = GetComponent<Health>();
        attacker = GetComponent<Attacker>();

        //Subscribing to events - PH, should be abstracted to not write this every time for a new enemy
        health.OnKnockbackReceived += movement.StartKnockback;
        health.OnDeath += () => 
        {
            Debug.Log($"{gameObject.name} is dead");
            Destroy(gameObject); //PH
        };

        //Not this one though
        movement.OnStateChange += HandleMovementStateChange;
    }

    private void Start()
    {
        movement.SetMoveSpeed(walkSpeed);
        player = GameObject.FindWithTag("Player"); //PH (probably not)
    }

    private void Update()
    {
        switch (movement.GetMovementState())
        {
            //case Movement.MovementState.Jumping:
            //    if (attacker.IsAttacking())
            //    {
            //        attacker.HandleLastingAttack();
            //    }
            //    else
            //    {
            //        attacker.StartLastingAttack(attackColl, attackColl.transform, movement.GetMoveDir());
            //    }
            //    break;
            case Movement.MovementState.Walking:
                if (Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 3 * 3 && attacker.CanAttack())
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
                break;
            default:
                break;
        }

        //animator.SetFloat("WalkDir", Shortcuts.GetAnimationDir(movement.GetWalkDir())); // Not PH
        animator.SetFloat("WalkDir", Shortcuts.GetAnimationDir(movement.GetMoveDir())); //PH
    }

    private void HandleMovementStateChange(Movement.MovementState from, Movement.MovementState to)
    {
        if (from == Movement.MovementState.Jumping)
        {
            attacker.StopLastingAttack();
        }

        if (to == Movement.MovementState.Jumping)
        {
            attacker.StartLastingAttack(attackColl, attackColl.transform, movement.GetMoveDir());
        }
    }
}