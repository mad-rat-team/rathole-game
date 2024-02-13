using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ratPrefab;
    
    private void Start()
    {
        if(GameManager.GetPlayer().GetComponent<PlayerCombat>().HasWeapon())
        {
            Instantiate(ratPrefab, transform.position, Quaternion.identity, RoomManager.GetCurrentRoom().transform);
            Destroy(this.gameObject);
        }
    }
}
