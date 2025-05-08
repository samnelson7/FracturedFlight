using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter; // how much time has passed since player ran off the edge

    [Header("Jump Buffer")]
    [SerializeField] private float bufferTime = 0.1f;
    private float bufferTimeRemaining = 0;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps = 0;
    private int jumpCounter = 0;

    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;

    BoxCollider2D playerCollider;

    private bool grounded = true;
    private bool isOnPlatform = false;
    public bool playerCanMove = true;
    private GameObject currentPlatform = null; 
    private Animator animator;
    private Rigidbody2D playerBody;
    private bool flightEnabled = false;
    private bool playerMoved = false;
    // vars for platform falling
    private float dropTimer = 0f;
    public float dropDuration = 0.5f;
    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerBody.freezeRotation = true;
        playerBody.gravityScale *= 2; 
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        animator.SetBool("Running", horizontalInput != 0);
        animator.SetBool("Grounded", grounded);
        if (!playerCanMove)
        {
            grounded = true;
            playerBody.velocity = new Vector2(playerBody.velocity.x/1.01f, playerBody.velocity.y);
            return;
        }
        playerBody.velocity = new Vector2(horizontalInput * runSpeed, playerBody.velocity.y);
        if (playerBody.velocity.y < -200) UIManager.instance.killed("You fell too fast and got dizzy");
        if (horizontalInput > 0)
        {
            playerMoved = true;
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (horizontalInput < 0)
        {
            playerMoved = true;
            transform.localScale = new Vector2(-1f,1f);
        }
        else // horizontal input is 0
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x / 30f, playerBody.velocity.y); // dampen horizontal movement when not pressing left/right
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (flightEnabled || grounded || coyoteCounter >= 0 || jumpCounter > 0) Jump();
            else bufferTimeRemaining = bufferTime;
        }
        // allow short hops
        if (Input.GetKeyUp(KeyCode.UpArrow) && playerBody.velocity.y > 0)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y / 2f);
        }
        if (playerBody.velocity.y > 35f && !Input.GetKey(KeyCode.UpArrow))
        {
            // Player is going up fast but not holding Up — apply damping
            playerBody.velocity = new Vector2(playerBody.velocity.x, playerBody.velocity.y * 0.8f);
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            DialogManager.instance.hideDialog();
        }
        if (grounded)
        {
            coyoteCounter = coyoteTime;
            if (extraJumps != 0) jumpCounter = extraJumps+1;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
            bufferTimeRemaining -= Time.deltaTime;
        }
        // platform falling
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Drop();
        }

        if (playerBody.velocity.y < -5) grounded = false;
        if (playerMoved) UIManager.instance.playerMoved();
    }
    private IEnumerator temporarilyIgnorePlatforms(float disableTime)
    {
        if (currentPlatform == null) yield break;

        Collider2D platformCollider = currentPlatform.GetComponent<Collider2D>();

        isOnPlatform = false;
        grounded = false;
        platformCollider.enabled = false;
        yield return new WaitForSeconds(disableTime);
        platformCollider.enabled = true;
    }
    public void Drop()
    {
        if (grounded && isOnPlatform)
        {
            StartCoroutine(temporarilyIgnorePlatforms(0.5f));
        }
    }
    private void Jump()
    {
        jumpCounter--;
        coyoteCounter = 0;
        playerBody.velocity = new Vector2(playerBody.velocity.x, jumpSpeed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            // Logic to check if the player is above the platform
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // only trigger grounded if we hit from the top
                {
                    grounded = true;

                    if (collision.gameObject.CompareTag("Platform"))
                    {
                        isOnPlatform = true;
                        currentPlatform = collision.gameObject;
                    }

                    if (bufferTimeRemaining >= 0)
                    {
                        Jump();
                    }

                    break;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
    }
}
