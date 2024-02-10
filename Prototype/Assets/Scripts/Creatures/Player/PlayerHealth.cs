using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Movement))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;

    private Health health;
    private Movement movement;

    private void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();

        health.OnHit += () => healthUI.UpdateHeartCount(health.GetHealthAmount());
        health.OnHealthChange += () => healthUI.UpdateHeartCount(health.GetHealthAmount());
        health.OnKnockbackReceived += movement.StartKnockback;
        health.OnDeath += () =>
        {
            GameManager.HandleDeath();
        };
    }

    private void Start()
    {
        healthUI.UpdateHeartCount(health.GetHealthAmount());
    }
}
