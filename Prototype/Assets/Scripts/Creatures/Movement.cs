using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// If <c>isMoving</c> is set to true, moves in the isoDirection of <c>target</c>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Transform jumpingPart; //A gameobject, all of the children of which should rise when the object is jumping
    private Vector3 jumpingPartOffset;

    [SerializeField] private Transform centerTransform; //NOTE: Maybe this should be changed to the point, where the object stands
    [SerializeField] [Range(0.01f, 2f)] private float moveDirChangeTime = 0.15f;
    [SerializeField] [Range(0.01f, 0.99f)] private float accCurveFlatness = 0.1f;

    public enum MovementState {
        Walking,
        Jumping,
        KnockedBack
    }

    private readonly float isMovingThreshold = 0.01f; // Minimum moveDir.SqrMagnitude value at which it is considered that the object moving

    private MovementState state;

    private bool isGrounded = true;
    private float jumpingPartVerticalVelocity;

    //private bool isWalking = true;
    private float moveSpeed = 1f;
    private Vector2 targetMoveDir = Vector2.zero;
    private Vector2 startMoveDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private float moveDirChangeFactor = 0f;

    private Vector2 jumpXYVelocity;
    private float jumpStartUpVelocity;
    private float jumpStartTime; //NOTE: It's possible to combine some jump and knockback variables, but I'll leave it as is for now to avoid confusion
    private float jumpTime;

    private Vector2 knockbackDir;
    private float knockbackTime;
    private float knockbackStartTime;
    private float knockbackStartVelocity;
    private float knockbackAcceleration;

    private Rigidbody2D rb;

    public void StartKnockback(Vector2 direction, float distance, float time)
    {
        if (state == MovementState.KnockedBack) return;
        state = MovementState.KnockedBack;

        knockbackTime = time;
        knockbackStartTime = Time.time;
        //knockbackDir = Shortcuts.NormalizeIso((Vector2)centerTransform.position - origin);
        knockbackDir = direction;
        knockbackStartVelocity = (2 * distance) / time;
        knockbackAcceleration = -(knockbackStartVelocity / time);
    }

    public void StartJump(Vector2 target, float time)
    {
        if (jumpingPart == null)
        {
            Debug.LogError($"GameObject {gameObject.name} can't jump. It's jumping part is not set.");
            return;
        }

        if (state != MovementState.Walking || !isGrounded) return;
        state = MovementState.Jumping;

        jumpStartTime = Time.time;
        jumpTime = time;
        jumpXYVelocity = (target - (Vector2)transform.position) / time;
        //jumpStartUpVelocity = Shortcuts.g * jumpTime / 2f;
        isGrounded = false;
        jumpingPartVerticalVelocity = Shortcuts.g * jumpTime / 2f;
    }

    /// <summary>
    /// Sets new target movement isoDirection.
    /// </summary>
    /// <returns>
    /// Movement isoDirection (iso-vector)
    /// </returns>
    /// <param name="newTargetMoveDir">New target move dir. Should be a normalized iso-vector.</param>
    public void SetTargetMoveDir(Vector2 newTargetMoveDir)
    {
        if (newTargetMoveDir != targetMoveDir)
        {
            startMoveDir = moveDir;
            moveDirChangeFactor = 0f;
            targetMoveDir = newTargetMoveDir;
        }
    }

    /// <summary>
    /// Sets the speed with which the gameObject moves in state "Moving".
    /// </summary>
    /// <param name="newMoveSpeed">New movement speed in units per second.</param>
    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public MovementState GetMovementState()
    {
        return state;
    }

    /// <summary>
    /// Returns walking isoDirection as an iso-vector. If the object is not in Walking state, returns the last isoDirection in which it was walking.
    /// </summary>
    /// <returns>An iso-vector</returns>
    public Vector2 GetWalkDir()
    {
        return moveDir;
    }

    /// <summary>
    /// Returns movement isoDirection as an iso-vector no matter the movement state.
    /// </summary>
    /// <returns>An iso-vector</returns>
    public Vector2 GetMoveDir()
    {
        switch (state)
        {
            case MovementState.Walking:
                return moveDir;
            case MovementState.Jumping:
                return Shortcuts.NormalizeIso(jumpXYVelocity);
            case MovementState.KnockedBack:
                return knockbackDir;
        }
        return Vector2.zero;
    }

    public bool IsMoving()
    {
        return moveDir.sqrMagnitude < isMovingThreshold;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (jumpingPart != null)
        {
            jumpingPartOffset = jumpingPart.localPosition; // If jumpingPart is set after Start() then this breaks but it's okay, I think
        }
    }

    private void FixedUpdate()
    {
        switch (state) {
            case MovementState.Walking:
                HandleWalking();
                break;
            case MovementState.Jumping:
                HandleJumping();
                break;
            case MovementState.KnockedBack:
                HandleKnockedBack();
                break;
        }

        if(jumpingPart != null && !isGrounded)
        {
            jumpingPartVerticalVelocity -= Shortcuts.g * Time.deltaTime / 2;
            jumpingPart.localPosition += Vector3.up * jumpingPartVerticalVelocity * Time.deltaTime;
            jumpingPartVerticalVelocity -= Shortcuts.g * Time.deltaTime - (Shortcuts.g * Time.deltaTime / 2);

            if(jumpingPart.localPosition.y <= jumpingPartOffset.y)
            {
                isGrounded = true;
                jumpingPart.localPosition = jumpingPartOffset;
                jumpingPartVerticalVelocity = 0f;
            }
        }
    }

    private void HandleWalking()
    {
        //if (!isWalking) return;

        float dirChangeSpeed = 1 / moveDirChangeTime;
        moveDirChangeFactor = Mathf.Clamp01(moveDirChangeFactor + Time.deltaTime * dirChangeSpeed);
        moveDir.x = SmootheningFunction(startMoveDir.x, targetMoveDir.x, moveDirChangeFactor);
        moveDir.y = SmootheningFunction(startMoveDir.y, targetMoveDir.y, moveDirChangeFactor);

        rb.velocity = moveDir * moveSpeed;
    }

    private void ResetMoveDir()
    {
        startMoveDir = Vector2.zero;
        targetMoveDir = Vector2.zero;
        moveDir = Vector2.zero;
        moveDirChangeFactor = 0f;
    }

    private void HandleJumping() //TODO
    {
        float timePassed = Time.time - jumpStartTime;
        //jumpingPart.localPosition = jumpingPartOffset + Vector3.up * (jumpStartUpVelocity * timePassed - (Shortcuts.g * timePassed * timePassed) / 2);
        rb.velocity = jumpXYVelocity;
        if (timePassed >= jumpTime)
        {
            //jumpingPart.localPosition = jumpingPartOffset;
            rb.velocity = Vector2.zero;

            state = MovementState.Walking;
            ResetMoveDir();
        }
    }

    private void HandleKnockedBack()
    {
        float timePassed = Time.time - knockbackStartTime;
        rb.velocity = knockbackDir * (knockbackStartVelocity + knockbackAcceleration * timePassed); //NOTE: Maybe this changes a little with different framerates

        if (timePassed >= knockbackTime)
        {
            rb.velocity = Vector2.zero;
            state = MovementState.Walking;
            ResetMoveDir();
        }
    }

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }
}
