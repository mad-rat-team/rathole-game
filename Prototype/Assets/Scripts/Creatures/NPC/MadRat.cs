using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCMovement))]
public class MadRat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NPCMovement movement;

    private void Awake()
    {
        movement = GetComponent<NPCMovement>();
    }

    private void Start()
    {
        movement.SetIsMoving(true);
    }

    private void Update()
    {
        if(Shortcuts.IsoToReal(player.transform.position - transform.position).sqrMagnitude < 3*3)
        {
            movement.SetTarget(player.transform.position);
            //Debug.Log("aa");
        }
        else
        {
            movement.SetTarget(transform.position);
        }

        GetComponentInChildren<SpriteRenderer>().flipX = movement.GetCurrentMoveDir().x > 0;
    }
}
