using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void KnockbackStartHandler(Vector2 origin, float distance, float time);
    public event KnockbackStartHandler OnKnockbackReceived;

    public void TakeHit(HitInfo hitInfo) // TODO: Make class HitInfo
    {
        OnKnockbackReceived?.Invoke(hitInfo.origin, hitInfo.knockbackDistance, hitInfo.knockbackTime);
    }
}
