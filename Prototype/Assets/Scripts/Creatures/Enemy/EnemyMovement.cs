using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If <c>isMoving</c> is set to true, moves in the direction of <c>target</c>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MovementWithChangingDir
{
    [SerializeField] private float moveSpeed;

    private Vector2 target;
    private bool isMoving;

    private bool knockedBack;
    private Vector2 knockbackDir;
    private float knockbackForce;

    private Rigidbody2D rb;

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void SetIsMoving(bool newIsMoving)
    {
        isMoving = newIsMoving;
    }

    public void StartKnockback(Vector2 origin, float force)
    {
        knockedBack = true;
        knockbackDir = Shortcuts.NormalizeIso((Vector2)transform.position - origin);
        knockbackForce = force;
    }

    public void EndKnockback()
    {
        knockedBack = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(knockedBack)
        {
            rb.velocity = knockbackDir * knockbackForce; // PLACEHOLDER
        }

        if (!isMoving)
        {
            return;
        }

        //Vector2 dirToTarget = (target - transform.position).normalized;
        Vector2 dirToTarget = Shortcuts.NormalizeIso(target - (Vector2)transform.position);
        //dirToTarget.y /= 2;
        UpdateMoveDir(dirToTarget);

        rb.velocity = GetIsoMoveDir() * moveSpeed;
        //Debug.Log($"{gameObject.name}: {GetIsoMoveDir() * moveSpeed}");
    }
}
