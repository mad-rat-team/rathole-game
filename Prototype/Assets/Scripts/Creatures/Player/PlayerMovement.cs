using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Movement))]
//[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject spriteObject;

    private Movement movement;
    private Animator animator;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        try
        {
            animator = spriteObject.GetComponent<Animator>();
        }
        catch
        {
            throw new System.Exception("Player's sprite object does not have an Animator component");
        }
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