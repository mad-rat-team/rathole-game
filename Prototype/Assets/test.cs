using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private float knockbackStartVelocity = 6f;
    [SerializeField] private float knockbackDistance = 3f;
    [SerializeField] private float knockbackTime = 0.5f;
    private float acceleration;

    private bool knockedBack = false;
    private float timePassed;
    private float startTime;

    private NPCHealth health;

    private void Awake()
    {
        
    }

    private void Start()
    {
        //acceleration = -(2 * (knockbackDistance + knockbackStartVelocity * knockbackTime)) / (knockbackTime * knockbackTime);
        acceleration = (2 * (knockbackStartVelocity * knockbackTime - knockbackDistance)) / (knockbackTime * knockbackTime);
        Debug.Log(acceleration);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            knockedBack = true;
            startTime = Time.time;
            acceleration = (2 * (knockbackStartVelocity * knockbackTime - knockbackDistance)) / (knockbackTime * knockbackTime);        }

        if(knockedBack)
        {
            timePassed = Time.time - startTime;
            GetComponent<Rigidbody2D>().velocity = Vector2.right * (knockbackStartVelocity + acceleration * timePassed);
            Debug.Log($"{timePassed}    {knockbackStartVelocity + acceleration * timePassed}");
            if(timePassed >= knockbackTime)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                knockedBack = false;
            }
        }
    }
}
