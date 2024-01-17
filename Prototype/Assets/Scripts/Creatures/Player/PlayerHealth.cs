using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Movement))]
public class PlayerHealth : MonoBehaviour
{
    private Health health;
    private Movement movement;

    private void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();

        health.OnKnockbackReceived += movement.StartKnockback;
        health.OnDeath += () =>
        {
            //PH
            Debug.Log("You died");
        };
    }
}
