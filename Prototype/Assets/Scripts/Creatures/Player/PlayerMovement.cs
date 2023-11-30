using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MovementWithChangingDir
{
    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //UpdateMoveDir(Shortcuts.RealToIso(GetInputMoveDir()));
        //rb.velocity = GetIsoMoveDir() * moveSpeed;

        rb.velocity = UpdateMoveDir(Shortcuts.RealToIso(GetInputMoveDir())) * moveSpeed;
    }

    //[SerializeField] private bool drawDebugStickGizmos = false;
    //private void OnDrawGizmos()
    //{
    //    if (!drawDebugStickGizmos) return;

    //    //stick
    //    Vector2 pos = new Vector2(-6f, 3f);
    //    float radCircle = 2f;
    //    float radStick = 0.5f;
    //    float radRealStick = 0.25f;

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(pos, radCircle);
    //    Gizmos.DrawSphere(pos + GetIsoMoveDir() * radCircle, radStick);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(pos + GetInputMoveDir() * radCircle, radRealStick);

    //    //Handles.Label(pos - new Vector2(1f, 2.25f), moveDir.ToString());
    //    //Handles.Label(pos - new Vector2(1f, 2.5f), moveDirChangeFactor.ToString());

    //    //graph
    //    //Gizmos.color = Color.white;
    //    //Vector2 graphPos = pos + new Vector2(0, -6);
    //    //Vector2 scale = new Vector2(2f, 2f);
    //    //float stepX = 0.05f;

    //    //Gizmos.DrawLine(graphPos, graphPos + new Vector2(scale.x, 0));
    //    //Gizmos.DrawLine(graphPos, graphPos + new Vector2(0, scale.y));

    //    //Vector2 prevPoint = Vector2.zero;
    //    //for (float x = 0f; x <= 1; x += stepX)
    //    //{
    //    //    Vector2 newPoint = new Vector2(x, SmootheningFunction(0, 1, x));
    //    //    Gizmos.DrawLine(graphPos + prevPoint * scale, graphPos + newPoint * scale);
    //    //    prevPoint = newPoint;
    //    //}
    //}


    /// <summary>
    /// Gets input move dir.
    /// </summary>
    /// <returns>
    /// <b>Non-iso-vector</b> of movement.
    /// </returns>
    private Vector2 GetInputMoveDir()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
}