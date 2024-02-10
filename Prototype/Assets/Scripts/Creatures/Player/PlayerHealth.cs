using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private Animator animator;
    [SerializeField] private float deathScreenDelay = 2f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeOutDelay = 0.5f;

    private Health health;
    private Movement movement;

    private bool alive = true;

    private void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();

        health.OnHealthChange += () => healthUI.UpdateHeartCount(health.GetHealthAmount());
        health.OnKnockbackReceived += movement.StartKnockback;
        health.OnDeath += () =>
        {
            if (!alive) return;
            alive = false;
            movement.ResetMoveDir();
            movement.enabled = false;
            GetComponent<PlayerCombat>().enabled = false;

            IEnumerator waitAndHandleDeathCoroutine()
            {
                yield return new WaitForSeconds(deathScreenDelay);
                GameManager.HandleDeath();
            }
            StartCoroutine(waitAndHandleDeathCoroutine());

            animator.SetTrigger("Died");
            ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutDuration, fadeOutDelay, false);
        };
    }

    private void Start()
    {
        healthUI.UpdateHeartCount(health.GetHealthAmount());
    }
}
