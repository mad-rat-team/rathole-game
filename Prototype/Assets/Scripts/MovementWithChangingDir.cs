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

    private float SmootheningFunction(float a, float b, float factor)
    {
        //return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness, factor)) / (1 - accCurveFlatness));
        return a + (b - a) * ((1 - Mathf.Pow(accCurveFlatness * accCurveFlatness, factor)) / (1 - accCurveFlatness * accCurveFlatness));
    }

    protected void UpdateMoveDir(Vector2 newTargetMoveDir) //Should be called every FixedUpdate step
    {
        if (newTargetMoveDir != targetMoveDir)
        {
            startMoveDir = moveDir;
            targetMoveDir = newTargetMoveDir;
            //moveDirChangeFactor = 0f;
            moveDirChangeFactor *= 
                newTargetMoveDir.sqrMagnitude == 0 || targetMoveDir.sqrMagnitude == 0 
                    ? 0f 
                    : Mathf.Clamp01(Vector2.Dot(targetMoveDir, newTargetMoveDir));
    }

    float dirChangeSpeed = 1 / moveDirChangeTime;
        moveDirChangeFactor = Mathf.Clamp01(moveDirChangeFactor + Time.deltaTime * dirChangeSpeed);
        moveDir.x = SmootheningFunction(startMoveDir.x, targetMoveDir.x, moveDirChangeFactor);
        moveDir.y = SmootheningFunction(startMoveDir.y, targetMoveDir.y, moveDirChangeFactor);
    }

    protected Vector2 GetMoveDir()
    {
        return moveDir;
    }
}