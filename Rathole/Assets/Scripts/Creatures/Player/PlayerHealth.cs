using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerCombat))]
[RequireComponent(typeof(PlayerInteractions))]
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private SpriteMask playerSpriteMask;
    [SerializeField] private Animator colorChangerAnimator;
    [SerializeField] private float deathScreenDelay = 2f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float fadeOutDelay = 0.5f;
    [SerializeField] private float corpseLen = 2f;
    [SerializeField] private LayerMask environmentLayerMask;

    private Health health;
    private Movement movement;

    private bool alive = true;

    public void SetHealthUIVisible(bool visible)
    {
        healthUI.SetHeartsVisible(visible);
    }

    private void Awake()
    {
        health = GetComponent<Health>();
        movement = GetComponent<Movement>();

        health.OnHealthChange += () => healthUI.UpdateHeartCount(health.GetHealthAmount());
        health.OnKnockbackReceived += movement.StartKnockback;
        health.OnDeath += HandleOnDeath;
        health.OnHit += HandleOnHit;
    }

    private void Start()
    {
        healthUI.UpdateHeartCount(health.GetHealthAmount());
    }

    private void Update()
    {
        playerSpriteMask.sprite = playerSprite.sprite;
    }

    private void HandleOnHit()
    {
        if (!alive) return;
        colorChangerAnimator.SetTrigger("Hit");
        SoundManager.PlaySoundEffect(SoundName.DamageGrunt);
    }

    private void HandleOnDeath()
    {
        if (!alive) return;
        alive = false;
        movement.ResetMoveDir();
        movement.enabled = false;
        GetComponent<PlayerCombat>().enabled = false;
        GetComponent<PlayerInteractions>().enabled = false;
        PauseManager.SetPaused(false);
        PauseManager.SetPauseMenuActive(false);
        PauseManager.SetManualPauseAllowed(false);

        IEnumerator waitAndHandleDeathCoroutine()
        {
            yield return new WaitForSeconds(deathScreenDelay);
            GameManager.HandleDeath();
            ScreenEffectManager.FadeFromCurrent(new Color(0, 0, 0, 0), 0.5f, 0f, true);
        }
        StartCoroutine(waitAndHandleDeathCoroutine());

        RaycastHit2D hitR = Physics2D.Raycast(transform.position, Vector2.right, corpseLen, environmentLayerMask);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position, Vector2.left, corpseLen, environmentLayerMask);
        bool dropToTheRight = hitR.collider == null || hitR.distance <= hitL.distance;

        animator.SetBool("DropToTheRight", dropToTheRight);
        animator.SetTrigger("Died");
        ScreenEffectManager.FadeFromCurrent(Color.black, fadeOutDuration, fadeOutDelay, false);
        SoundManager.FadeOutSoundtrack(fadeOutDuration + fadeOutDelay);
    }
}
