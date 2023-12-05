using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts
{
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
}
