using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    [SerializeField] Transform spriteTransform;
    [SerializeField] float time = 0.5f;

    //private Vector2 velocity;
    private bool isJumping;
    private float startUpVelocity;
    private float startTime;
    private float timePassed;
    private float jumpTime;
    private const float g = 9.81f / 1.5f;

    private Vector3 spriteOffset;

    private void Start()
    {
        spriteOffset = spriteTransform.localPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !isJumping)
        {
            Vector2 destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetComponent<Rigidbody2D>().velocity = (destination - (Vector2)transform.position) / time;
            
            isJumping = true;
            jumpTime = time;
            startTime = Time.time;
            timePassed = 0f;
            startUpVelocity = g * time / 2; 
        }

        if(isJumping)
        {
            timePassed = Time.time - startTime;
            spriteTransform.localPosition = spriteOffset + Vector3.up * (startUpVelocity * timePassed - (g*timePassed*timePassed)/2);
            if(timePassed >= jumpTime)
            {
                spriteTransform.localPosition = spriteOffset;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                isJumping = false;
            }
        }
    }
}
