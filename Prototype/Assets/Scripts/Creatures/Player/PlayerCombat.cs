using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject attackTrailRotator;
    [SerializeField] private GameObject attackTrail;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ContactFilter2D enemiesContactFilter;
    [SerializeField] private AttackStats playerAttackStats;
    [SerializeField] private float attackCoolown = 0.5f;

    private float lastAttackTime;
    private Collider2D attackTrailColl;
    private Animator attackTrailAnimator;

    private void Awake()
    {
        if (!attackTrail.TryGetComponent<Collider2D>(out attackTrailColl))
        {
            Debug.LogError("attackTrail does not have a Collider2D component");
        }
        
        if (!attackTrail.TryGetComponent<Animator>(out attackTrailAnimator))
        {
            Debug.LogError("attackTrail does not have an Animator component");
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - lastAttackTime >= attackCoolown)
            {
                lastAttackTime = Time.time;
                // Casting camera world pos to Vector2 is necessary so that its Z component doesn't affect the calculation
                Vector2 dir = ((Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)attackTrailRotator.transform.position).normalized;
                Attack(dir);
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack(Vector2 dir)
    {
        //Debug.Log($"Attacked to {dir}");
        float attackAngle = Vector2.SignedAngle(Vector2.right, dir);

        attackTrailRotator.transform.eulerAngles = Vector3.forward * attackAngle;

        //attackTrailRotator.transform.localScale =
        //    new Vector3(
        //        attackTrailRotator.transform.localScale.x,
        //        Mathf.Abs(attackAngle) > 90 ? -1 : 1,
        //        attackTrailRotator.transform.localScale.z
        //        );

        Physics2D.SyncTransforms(); // Force update collider's position according to attackTrailRotator's rotation and scale
        List<Collider2D> enemyColliders = new List<Collider2D>();
        attackTrailColl.OverlapCollider(enemiesContactFilter, enemyColliders);

        foreach (var enemyColl in enemyColliders)
        {
            Health enemyHealth = enemyColl.GetComponentInParent<Health>();

            HitInfo hitInfo = new HitInfo(attackTrailRotator.transform.position, Shortcuts.NormalizeIso(dir), playerAttackStats);
            //hitInfo.origin = attackTrailRotator.transform.position;
            //hitInfo.knockbackDistance = 3f;
            //hitInfo.knockbackTime = 0.5f;
            enemyHealth.TakeHit(hitInfo);
        }

        attackTrailAnimator.SetTrigger("Attacked");
    }
}
