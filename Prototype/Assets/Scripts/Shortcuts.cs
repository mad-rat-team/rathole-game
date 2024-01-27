using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts
{
    public const float g = 9.81f / 1.5f; // MAYBE TODO: Make convertion functions from meters to units and backwards for all axis

    public static Vector2 IsoToReal(Vector2 isoVec)
    {
        isoVec.y *= 2;
        return isoVec;
    }
    
    public static Vector2 RealToIso(Vector2 realVec)
    {
        realVec.y /= 2;
        return realVec;
    }

    /// <summary>
    /// Converts <paramref name="isoVec"/> to a real vector, normalizes it and converts it back to an iso vector.
    /// </summary>
    /// <param name="isoVec"></param>
    /// <returns></returns>
    public static Vector2 NormalizeIso(Vector2 isoVec)
    {
        isoVec.y *= 2;
        isoVec = isoVec.normalized;
        isoVec.y /= 2;
        return isoVec;
    }

    public static float GetAnimationDir(Vector2 dir)
    {
        return ((Vector2.SignedAngle(Vector2.right, dir) + 360f) % 360f) / 360f;
    }
}
