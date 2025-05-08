using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class LevelCompleteJumping : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidBody2d;
    public bool complete = false;
    private bool grounded = true;
    private float motionlessCounter = 0;
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        rigidBody2d.gravityScale *= 2;
    }
    void Update()
    {
        if (animator.GetBool("EndReached") && !complete)
        {
            grounded = false;
            animator.SetBool ("Grounded", grounded);
            complete = true;
            rigidBody2d.velocity = new Vector2(0,35);
        }
        if (complete && motionlessCounter >= 1.5)
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rigidBody2d.velocity = new Vector2(0, 35);
            motionlessCounter = 0;
        }
        if (complete) motionlessCounter += Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
    }
}
