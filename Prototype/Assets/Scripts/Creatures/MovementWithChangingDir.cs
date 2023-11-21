using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementWithChangingDir : MonoBehaviour
{
    [SerializeField][Range(0.01f, 2f)] private float moveDirChangeTime = 0.15f;
    [SerializeField][Range(0.01f, 0.99f)] private float accCurveFlatness = 0.1f;

    private Vector2 startMoveDir = Vector2.zero;
    private Vector2 targetMoveDir = Vector2.zero;
    private Vector2 moveDir = Vector2.zero;
    private float moveDirChangeFactor = 0f;
    
    /// <summary>
    /// Should be called every frame.
    /// </summary>
    /// <returns>
    /// Movement direction (iso-vector)
    /// </returns>
    /// <param name="newTargetIsoMoveDir">New target move dir. Should be an iso-vector.</param>
    protected Vector2 GetMoveDir(Vector2 newTargetIsoMoveDir) //Should be called every FixedUpdate step
    {
        if (newTargetIsoMoveDir != targetMoveDir)
        {
            startMoveDir = moveDir;
            moveDirChangeFactor = 0f;
            //moveDirChangeFactor *=
            //    newTargetIsoMoveDir.sqrMagnitude == 0 || targetMoveDir.sqrMagnitude == 0
            //        ? 0f
            //        : Mathf.Clamp01(Vector2.Dot(targetMoveDir, newTargetIsoMoveDir));
            //moveDirChangeFactor *=
            //    newTargetIsoMoveDir.sqrMagnitude == 0 || targetMoveDir.sqrMagnitude == 0
            //        ? 0f
            //        : Mathf.Clamp01(Vector2.Dot(Shortcuts.IsoToReal(targetMoveDir), Shortcuts.IsoToReal(newTargetIsoMoveDir)));
            //Debug.Log($"old: {targetMoveDir}; new: {newTargetIsoMoveDir}");
            //Debug.Log(moveDirChangeFactor);

            targetMoveDir = newTargetIsoMoveDir;
            //moveDirChangeFactor *=
            //    newTargetIsoMoveDir.sqrMagnitude == 0 || targetMoveDir.sqrMagnitude == 0
            //        ? 0f
            //        : Mathf.Clamp01(Mathf.Cos(Vector2.Angle(Shortcuts.IsoToReal(targetMoveDir), Shortcuts.IsoToReal(newTargetIsoMoveDir)) * Mathf.Deg2Rad));

        }

        float dirChangeSpeed = 1 / moveDirChangeTime;
        moveDirChangeFactor = Mathf.Clamp01(moveDirChangeFactor + Time.deltaTime * dirChangeSpeed);
        moveDir.x = SmootheningFunction(startMoveDir.x, targetMoveDir.x, moveDirChangeFactor);
        moveDir.y = SmootheningFunction(startMoveDir.y, targetMoveDir.y, moveDirChangeFactor);

        return moveDir;
    }

    protected Vector2 GetMoveDir()
    {
        return moveDir;
    }

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }
}