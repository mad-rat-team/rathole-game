using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit(Vector2 origin, float knockback) // TODO: Make class HitInfo
    {
        rb.AddForce(((Vector2)transform.position - origin).normalized * knockback);
    }
}
