using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class MovementWithChangingDir : MonoBehaviour
{
    [SerializeField][Range(0.01f, 2f)] private float moveDirChangeTime = 0.15f;
    [SerializeField][Range(0.01f, 0.99f)] private float accCurveFlatness = 0.1f;

    private Vector2 startMoveDir = Vector2.zero;
    private Vector2 targetMoveDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private float moveDirChangeFactor = 0f;
    
    /// <summary>
    /// Should be called every Fixed update step.
    /// </summary>
    /// <returns>
    /// Movement direction (iso-vector)
    /// </returns>
    /// <param name="newTargetIsoMoveDir">New target move dir. Should be a normalized iso-vector.</param>
    protected Vector2 UpdateMoveDir(Vector2 newTargetIsoMoveDir)
    {
        if (newTargetIsoMoveDir != targetMoveDir)
        {
            startMoveDir = moveDir;
            moveDirChangeFactor = 0f;
            targetMoveDir = newTargetIsoMoveDir;

        }

        float dirChangeSpeed = 1 / moveDirChangeTime;
        moveDirChangeFactor = Mathf.Clamp01(moveDirChangeFactor + Time.deltaTime * dirChangeSpeed);
        moveDir.x = SmootheningFunction(startMoveDir.x, targetMoveDir.x, moveDirChangeFactor);
        moveDir.y = SmootheningFunction(startMoveDir.y, targetMoveDir.y, moveDirChangeFactor);

        return moveDir;
    }

    protected void ResetMoveDir()
    {
        startMoveDir = Vector2.zero;
        targetMoveDir = Vector2.zero;
        moveDir = Vector2.zero;
        moveDirChangeFactor = 0f;
    }

    public virtual Vector2 GetMoveDir()
    {
        return moveDir;
    }

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }
}