using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo
{
    public Vector2 origin;
    public Vector2 isoDirection;
    //public float knockbackDistance;
    //public float knockbackTime;
    public readonly AttackStats attackStats;

    public HitInfo(Vector2 origin, Vector2 isoDirection, AttackStats attackStats)
    {
        this.origin = origin;
        this.isoDirection = isoDirection;
        this.attackStats = attackStats;
    }
}

[System.Serializable]
public class AttackStats
{
    public int damage;
    public bool hasKnockback;
    public float knockbackDistance;
    public float knockbackTime;
}
