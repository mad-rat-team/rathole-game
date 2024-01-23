using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject attackTrailRotator;
    [SerializeField] private GameObject attackTrail;
    [SerializeField] private Camera playerCamera;

    private Attacker attacker;
    private Collider2D attackTrailColl;
    private Animator attackTrailAnimator;

    private void Awake()
    {
        attacker = GetComponent<Attacker>();

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
        if(InputManager.GetButtonDown(InputManager.InputButton.Attack) || InputManager.GetButton(InputManager.InputButton.Attack))
        {
            if (attacker.CanAttack())
            {
                // Casting camera world pos to Vector2 is necessary so that its Z component doesn't affect the calculation
                Vector2 dir = ((Vector2)playerCamera.ScreenToWorldPoint(Input.mousePosition) - (Vector2)attackTrailRotator.transform.position).normalized;
                Attack(dir);
            }
        }
    }

    private void Attack(Vector2 dir)
    {
        float attackAngle = Vector2.SignedAngle(Vector2.right, dir);
        attackTrailRotator.transform.eulerAngles = Vector3.forward * attackAngle;
        Physics2D.SyncTransforms(); // Force update collider's position according to attackTrailRotator's rotation and scale

        attacker.FixedDurationAttack(attackTrailColl, attackTrailRotator.transform, Shortcuts.RealToIso(dir));

        attackTrailAnimator.SetTrigger("Attacked");
    }
}
