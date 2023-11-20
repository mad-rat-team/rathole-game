using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If <c>isMoving</c> is set to true, moves in the direction of <c>target</c>
/// </summary>
public class EnemyMovement : MovementWithChangingDir
{
    [SerializeField] private float moveSpeed;
    private Vector3 target;
    private bool isMoving;

    private Rigidbody2D rb;

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public void SetIsMoving(bool newIsMoving)
    {
        isMoving = newIsMoving;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isMoving)
        {
            return;
        }

        //Vector2 dirToTarget = (target - transform.position).normalized;
        Vector2 dirToTarget = Shortcuts.NormalizeIso(target - transform.position);
        //dirToTarget.y /= 2;
        UpdateMoveDir(dirToTarget);

        rb.velocity = GetIsoMoveDir() * moveSpeed;
        //Debug.Log($"{gameObject.name}: {GetIsoMoveDir() * moveSpeed}");
    }
}
