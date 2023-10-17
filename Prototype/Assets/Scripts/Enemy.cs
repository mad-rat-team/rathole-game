using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float visibleRange;

    private float knockbackTimeLeft = 0f;
    private bool knockedBack = false;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(knockedBack)
        {
            knockbackTimeLeft -= Time.deltaTime;
            if (knockbackTimeLeft > 0)
            {
                return;
            }
            else
            {
                knockedBack = false;
            }
        }

        Vector2 vectorToPlayer = player.transform.position - transform.position;
        Vector2 moveDir = (Vector2.SqrMagnitude(vectorToPlayer) <= visibleRange * visibleRange)
            ? vectorToPlayer.normalized:
            Vector2.zero;

        moveDir.y /= 2;
        rb.velocity = moveDir * moveSpeed;
    }

    public void TakeHit(Vector2 origin, float knockbackForce, float knockbackTime) // TODO: Make class HitInfo
    {
        rb.AddForce(((Vector2)transform.position - origin).normalized * knockbackForce);
        knockedBack = true;
        knockbackTimeLeft = knockbackTime;
    }
}
