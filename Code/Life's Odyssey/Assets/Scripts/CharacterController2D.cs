using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed ; // character movement speed
    [SerializeField] float runningSpeed; // character running speed
    Vector2 motionVector;
    public Vector2 lastMotionVector;
    Animator animator;
    public bool moving; // is the character moving?

    bool running;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // if left shift is pressed and character is not exhausted, start running
        if(Input.GetKeyDown(KeyCode.LeftShift) && GetComponent<Character>().isExhausted==false)
        {
            running = true;
        }
        // if left shift is released or character is exhausted, stop running
        if(Input.GetKeyUp(KeyCode.LeftShift) || GetComponent<Character>().isExhausted==true)
        {
            running = false;
        }   
        // get raw input from the horizontal and vertical axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // set the motion vector based on input
        motionVector.x = horizontal;
        motionVector.y = vertical;
        motionVector = new Vector2( 
            horizontal,
            vertical
            );

        // set the animator with current input values
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        // in case character stops keep last moving vector
        if(horizontal!=0 || vertical != 0)
        {
            lastMotionVector = new Vector2(
                horizontal,
                vertical
                ).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }

    void FixedUpdate()
    {
        // move character based on current motion vector
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * (running==true ? runningSpeed : speed);
    }
}
