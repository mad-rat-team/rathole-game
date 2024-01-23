using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;

    private int currentHealth;

    public event Action OnHit;
    public event Action OnDeath;


    public delegate void KnockbackStartHandler(Vector2 direction, float distance, float time);
    public event KnockbackStartHandler OnKnockbackReceived;

    public void TakeHit(HitInfo hitInfo)
    {
        OnHit?.Invoke();
        currentHealth -= hitInfo.attackStats.damage;
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }

        if (hitInfo.attackStats.hasKnockback)
        {
            OnKnockbackReceived?.Invoke(hitInfo.isoDirection, hitInfo.attackStats.knockbackDistance, hitInfo.attackStats.knockbackTime);
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
}
