using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Animator animator;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Start()
    {
        movement.SetMoveSpeed(moveSpeed);
    }

    private void Update()
    {
        movement.SetTargetMoveDir(Shortcuts.RealToIso(InputManager.GetInputMoveDir()));

        animator.SetFloat("WalkDir", Shortcuts.GetAnimationDir(movement.GetWalkDir()));
        animator.SetBool("IsWalking", movement.GetMovementState() == Movement.MovementState.Walking && !movement.IsMoving());
    }
}