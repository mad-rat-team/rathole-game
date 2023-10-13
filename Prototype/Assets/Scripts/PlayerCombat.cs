using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject attackObject;
    [SerializeField] LayerMask enemiesLayerMask;
    [SerializeField] Collider2D attackColl;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Casting camera world pos to Vector2 is necessary so that its Z component doesn't affect the calculation
            Vector2 dir = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)attackObject.transform.position).normalized;
            Attack(dir);
        }

        Debug.DrawRay(
            attackObject.transform.position,
            ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)(attackObject.transform.position)).normalized,
            Color.white
            );
    }

    private void Attack(Vector2 dir)
    {
        Debug.Log($"Attacked to {dir}");
        attackObject.transform.eulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.right, dir);
        attackObject.GetComponentInChildren<Animator>().SetTrigger("Attacked"); // Delete this ASAP; Make attackObject handle its animation;
    }
}
