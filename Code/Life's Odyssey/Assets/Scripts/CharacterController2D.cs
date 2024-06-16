using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 5f; // character movement speed
    Vector2 motionVector;
    public Vector2 lastMotionVector;
    Animator animator;
    public bool moving; // is the character moving?

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
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
        rigidbody2d.velocity = motionVector * speed;
    }
}
