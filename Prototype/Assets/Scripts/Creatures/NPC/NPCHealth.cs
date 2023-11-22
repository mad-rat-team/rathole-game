using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    public event Action<Vector2, float> OnKnockbackReceived; // <knockbackForce>
    public event Action OnKnockbackEnded;

    //[SerializeField] private float maxHealth = 20f;

    //private float currentHealth;
    private float knockbackTimeLeft = 0f;
    private bool knockedBack = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (knockedBack)
        {
            knockbackTimeLeft -= Time.deltaTime;
            if (knockbackTimeLeft > 0)
            {
                return;
            }
            else
            {
                OnKnockbackEnded?.Invoke();
                knockedBack = false;
            }
        }
    }

    public void TakeHit(Vector2 origin, float knockbackForce, float knockbackTime) // TODO: Make class HitInfo
    {
        //rb.AddForce(((Vector2)transform.position - origin).normalized * knockbackForce);
        knockedBack = true;
        knockbackTimeLeft = knockbackTime;
        OnKnockbackReceived?.Invoke(origin, knockbackForce);
    }
}
