using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField][Range(0.01f, 2f)] private float moveDirChangeTime = 0.15f;
    [SerializeField][Range(0.01f, 0.99f)] private float accCurveFlatness = 0.1f;
    //[SerializeField] ContactFilter2D obstaclesContactFilter;
    [SerializeField] private bool drawDebugStickGizmos = false;

    private Vector2 inputMoveDir = Vector2.zero;
    private Vector2 startMoveDir = Vector2.zero;
    private Vector2 targetMoveDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private float moveDirChangeFactor = 0f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateInputMoveDir();
    }

    private void FixedUpdate()
    {
        UpdateMoveDir();
        rb.velocity = moveDir * moveSpeed;
    }

    private void OnDrawGizmos()
    {
        if (!drawDebugStickGizmos) return;

        //stick
        Vector2 pos = new Vector2(-6f, 3f);
        float radCircle = 2f;
        float radStick = 0.5f;
        float radRealStick = 0.25f;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pos, radCircle);
        Gizmos.DrawSphere(pos + moveDir * radCircle, radStick);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos + inputMoveDir * radCircle, radRealStick);

        Handles.Label(pos - new Vector2(1f, 2.25f), moveDir.ToString());
        Handles.Label(pos - new Vector2(1f, 2.5f), moveDirChangeFactor.ToString());

        //graph
        Gizmos.color = Color.white;
        Vector2 graphPos = pos + new Vector2(0, -6);
        Vector2 scale = new Vector2(2f, 2f);
        float stepX = 0.05f;

        Gizmos.DrawLine(graphPos, graphPos + new Vector2(scale.x, 0));
        Gizmos.DrawLine(graphPos, graphPos + new Vector2(0, scale.y));

        Vector2 prevPoint = Vector2.zero;
        for(float x = 0f; x <= 1; x += stepX)
        {
            Vector2 newPoint = new Vector2(x, SmootheningFunction(0, 1, x));
            Gizmos.DrawLine(graphPos + prevPoint * scale, graphPos + newPoint * scale);
            prevPoint = newPoint;
        }
    }

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }

    private void UpdateInputMoveDir()
    {
        inputMoveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void UpdateMoveDir()
    {
        if (inputMoveDir != targetMoveDir)
        {
            startMoveDir = moveDir;
            targetMoveDir = inputMoveDir;
            // due to isometric view, y should be 2 times less than x
            targetMoveDir.y /= 2;
            moveDirChangeFactor = 0f;
        }

        float dirChangeSpeed = 1 / moveDirChangeTime;
        moveDirChangeFactor = Mathf.Clamp01(moveDirChangeFactor + Time.deltaTime * dirChangeSpeed);
        moveDir.x = SmootheningFunction(startMoveDir.x, targetMoveDir.x, moveDirChangeFactor);
        moveDir.y = SmootheningFunction(startMoveDir.y, targetMoveDir.y, moveDirChangeFactor);
    }
}