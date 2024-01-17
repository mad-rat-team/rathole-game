using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private LayerMask enemyHitboxLayerMask;
    [SerializeField] private AttackStats attackStats;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float lastingAttackDuration = 0.1f;

    private ContactFilter2D enemyHitboxContactFilter;

    private float lastAttackTime;
    private bool isAttacking;
    private Collider2D lastingAttackColl;
    private Transform lastingAttackOrigin;
    private Vector2 lastingAttackIsoDir;
    private HashSet<Collider2D> hitEnemyColliders = new();

    private bool fixedDurationAttackActive;
    private float fixedDurationAttackEndTime;

    public void OneFrameAttack(Collider2D attackColl, Vector2 attackOrigin, Vector2 attackIsoDir)
    {
        if (!CanAttack()) return;
        lastAttackTime = Time.time;
        List<Collider2D> enemyColliders = new List<Collider2D>();
        attackColl.OverlapCollider(enemyHitboxContactFilter, enemyColliders);

        foreach (Collider2D enemyColl in enemyColliders)
        {
            Health enemyHealth = enemyColl.GetComponentInParent<Health>();

            HitInfo hitInfo = new HitInfo(attackOrigin, attackIsoDir, attackStats);
            enemyHealth.TakeHit(hitInfo);
        }
    }

    public void FixedDurationAttack(Collider2D attackColl, Transform attackOrigin, Vector2 attackIsoDir)
    {
        StartLastingAttack(attackColl, attackOrigin, attackIsoDir);
        fixedDurationAttackActive = true;
        fixedDurationAttackEndTime = Time.time + lastingAttackDuration;
    }

    public void StartLastingAttack(Collider2D attackColl, Transform attackOrigin, Vector2 attackIsoDir)
    {
        if (!CanAttack()) return;
        isAttacking = true;
        lastingAttackColl = attackColl;
        lastingAttackOrigin = attackOrigin;
        lastingAttackIsoDir = attackIsoDir;
        hitEnemyColliders.Clear();
    }

    private void HandleLastingAttack()
    {
        if (!isAttacking)
        {
            Debug.LogWarning("Cannot handle lasting attack if it has not been started");
            return;
        }

        List<Collider2D> enemyColliders = new List<Collider2D>();
        lastingAttackColl.OverlapCollider(enemyHitboxContactFilter, enemyColliders);

        foreach (Collider2D enemyColl in enemyColliders)
        {
            if (hitEnemyColliders.Contains(enemyColl)) continue;

            Health enemyHealth = enemyColl.GetComponentInParent<Health>();

            HitInfo hitInfo = new HitInfo(lastingAttackOrigin.position, lastingAttackIsoDir, attackStats);
            enemyHealth.TakeHit(hitInfo);
            hitEnemyColliders.Add(enemyColl);
        }
    }

    public void StopLastingAttack()
    {
        if (!isAttacking) return;
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown && !isAttacking;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private void Awake()
    {
        enemyHitboxContactFilter = new ContactFilter2D();
        enemyHitboxContactFilter.layerMask = enemyHitboxLayerMask;
        enemyHitboxContactFilter.useLayerMask = true;
    }

    private void Update()
    {
        if (fixedDurationAttackActive)
        {
            if (Time.time >= fixedDurationAttackEndTime)
            {
                StopLastingAttack();
                fixedDurationAttackActive = false;
            }
        }

        if (isAttacking)
        {
            HandleLastingAttack();
        }
    }
}
