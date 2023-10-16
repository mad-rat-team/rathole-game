using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject attackTrail;
    [SerializeField] SpriteRenderer attackTrailSpriteRenderer;
    [SerializeField] Camera playerCamera;
    [SerializeField] ContactFilter2D enemiesContactFilter;

    private Collider2D attackColl;
    private Animator attackTrailAnimator;

    private void Awake()
    {
        if (!attackTrail.TryGetComponent<Collider2D>(out attackColl))
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
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Casting camera world pos to Vector2 is necessary so that its Z component doesn't affect the calculation
            Vector2 dir = ((Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)attackTrail.transform.position).normalized;
            Attack(dir);
        }

        //Debug.DrawRay(
        //    attackTrail.transform.position,
        //    ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)(attackTrail.transform.position)).normalized,
        //    Color.white
        //    );
    }

    private void Attack(Vector2 dir)
    {
        //Debug.Log($"Attacked to {dir}");
        float attackAngle = Vector2.SignedAngle(Vector2.right, dir);
        attackTrail.transform.eulerAngles = Vector3.forward * attackAngle;

        Physics2D.SyncTransforms(); // Force update collider's position according to attackTrail's rotation
        List<Collider2D> enemyColliders = new List<Collider2D>();
        int enemyColliderCount = attackColl.OverlapCollider(enemiesContactFilter, enemyColliders);

        foreach (var enemyColl in enemyColliders)
        {
            Enemy enemy = enemyColl.GetComponent<Enemy>();
            enemy.TakeHit(attackTrail.transform.position, 1000f);
        }

        attackTrailSpriteRenderer.flipY = Mathf.Abs(attackAngle) > 90;
        attackTrailAnimator.SetTrigger("Attacked");
    }
}
