using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private LayerMask enemyHitboxLayerMask;
    [SerializeField] private AttackStats attackStats;
    [SerializeField] private float attackCoolown = 0.5f;

    private ContactFilter2D enemyHitboxContactFilter;

    private float lastAttackTime;
    private bool isAttacking;
    private Collider2D lastingAttackColl;
    private Vector2 lastingAttackOrigin;
    private Vector2 lastingAttackIsoDir;
    private HashSet<Collider2D> damagedColliders = new();

    public void QuickAttack(Collider2D attackColl, Vector2 attackOrigin, Vector2 attackIsoDir)
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

    public void StartLastingAttack(Collider2D attackColl, Vector2 attackOrigin, Vector2 attackIsoDir)
    {
        if (!CanAttack()) return;
        isAttacking = true;
        lastingAttackColl = attackColl;
        lastingAttackOrigin = attackOrigin;
        lastingAttackIsoDir = attackIsoDir;
        damagedColliders.Clear();
    }

    public void UpdateLastingAttackOrigin(Vector2 newAttackOrigin)
    {
        lastingAttackOrigin = newAttackOrigin;
    }

    public void UpdateLastingAttackDir(Vector2 newAttackIsoDir)
    {
        lastingAttackIsoDir = newAttackIsoDir;
    }

    public void HandleLastingAttack()
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
            if (damagedColliders.Contains(enemyColl)) continue;
            Health enemyHealth = enemyColl.GetComponentInParent<Health>();

            HitInfo hitInfo = new HitInfo(lastingAttackOrigin, lastingAttackIsoDir, attackStats);
            enemyHealth.TakeHit(hitInfo);
            damagedColliders.Add(enemyColl);
        }
    }

    public void StopLastingAttack()
    {
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCoolown && !isAttacking;
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
}
