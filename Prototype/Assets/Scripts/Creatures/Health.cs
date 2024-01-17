using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void KnockbackStartHandler(Vector2 direction, float distance, float time);
    public event KnockbackStartHandler OnKnockbackReceived;

    public void TakeHit(HitInfo hitInfo)
    {
        //OnKnockbackReceived?.Invoke(hitInfo.origin, hitInfo.attackStats.knockbackDistance, hitInfo.attackStats.knockbackTime);
        OnKnockbackReceived?.Invoke(hitInfo.isoDirection, hitInfo.attackStats.knockbackDistance, hitInfo.attackStats.knockbackTime);
    }
}
