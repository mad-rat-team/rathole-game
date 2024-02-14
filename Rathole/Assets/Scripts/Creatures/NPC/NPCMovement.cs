using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCMovement : MovementWithChangingDir
{
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float moveSpeed;

    private Vector2 target;
    private bool isMoving = true;

    private bool knockedBack;
    private Vector2 knockbackDir;
    private float knockbackTime;
    private float knockbackStartTime;
    private float knockbackStartVelocity;
    private float knockbackAcceleration;

    private Rigidbody2D rb;

    public void SetTarget(Vector2 newTarget)
    {
        target = newTarget;
    }

    public void SetIsMoving(bool newIsMoving)
    {
        isMoving = newIsMoving;
    }

    public void StartKnockback(Vector2 origin, float distance, float time)
    {
        knockedBack = true;
        knockbackTime = time;
        knockbackStartTime = Time.time;
        knockbackDir = Shortcuts.NormalizeIso((Vector2)hitbox.transform.position - origin);
        knockbackStartVelocity = (2 * distance) / time;
        knockbackAcceleration = -(knockbackStartVelocity / time);
    }

    public override Vector2 GetMoveDir()
    {
        if (knockedBack)
        {
            return knockbackDir;
        }
        return base.GetMoveDir();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockedBack)
        {
            float timePassed = Time.time - knockbackStartTime;
            rb.velocity = knockbackDir * (knockbackStartVelocity + knockbackAcceleration * timePassed); //NOTE: Maybe this changes a little with different framerates

            if (timePassed >= knockbackTime)
            {
                knockedBack = false;
                rb.velocity = Vector2.zero;
                ResetMoveDir();
            }
            else
            {
                return;
            }
        }

        Vector2 newTargetMoveDir;

        if (!isMoving)
        {
            newTargetMoveDir = Vector2.zero;
        }
        else
        {
            newTargetMoveDir = Shortcuts.NormalizeIso(target - (Vector2)transform.position);
        }

        rb.velocity = UpdateMoveDir(newTargetMoveDir) * moveSpeed;
    }
}
