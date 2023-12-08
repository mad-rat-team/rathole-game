using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If <c>isMoving</c> is set to true, moves in the direction of <c>target</c>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Transform centerTransform;
    [SerializeField] [Range(0.01f, 2f)] private float moveDirChangeTime = 0.15f;
    [SerializeField] [Range(0.01f, 0.99f)] private float accCurveFlatness = 0.1f;

    public enum MovementState {
        None,
        Moving,
        Jumping,
        KnockedBack
    }

    private MovementState state;

    private float moveSpeed = 1f;
    private Vector2 targetMoveDir = Vector2.zero;
    private Vector2 startMoveDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private float moveDirChangeFactor = 0f;

    private Vector2 knockbackDir;
    private float knockbackTime;
    private float knockbackStartTime;
    private float knockbackStartVelocity;
    private float knockbackAcceleration;

    private Rigidbody2D rb;

    /// <summary>
    /// Starts moving with the speed set by SetMoveSpeed
    /// </summary>
    public void StartMoving()
    {
        state = MovementState.Moving;
        ResetMoveDir();
    }

    public void StartKnockback(Vector2 origin, float distance, float time)
    {
        state = MovementState.KnockedBack;
        knockbackTime = time;
        knockbackStartTime = Time.time;
        knockbackDir = Shortcuts.NormalizeIso((Vector2)centerTransform.position - origin);
        knockbackStartVelocity = (2 * distance) / time;
        knockbackAcceleration = -(knockbackStartVelocity / time);
    }

    /// <summary>
    /// Sets new target movement direction.
    /// </summary>
    /// <returns>
    /// Movement direction (iso-vector)
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch(state) {
            case MovementState.Moving:
                HandleMoving();
                break;
            case MovementState.Jumping:
                HandleJumping();
                break;
            case MovementState.KnockedBack:
                HandleKnockedBack();
                break;
        }
    }

    private void HandleMoving()
    {
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

    private void HandleJumping()
    {

    }

    private void HandleKnockedBack()
    {
        float timePassed = Time.time - knockbackStartTime;
        rb.velocity = knockbackDir * (knockbackStartVelocity + knockbackAcceleration * timePassed); //NOTE: Maybe this changes a little with different framerates

        if (timePassed >= knockbackTime)
        {
            rb.velocity = Vector2.zero;
            StartMoving();
        }
    }

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }
}
