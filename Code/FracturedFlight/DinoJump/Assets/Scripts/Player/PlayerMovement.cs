using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter; // how much time has passed since player ran off the edge

    [Header("Jump Buffer")]
    [SerializeField] private float bufferTime = 0.1f;
    private float bufferJumpTimeRemaining = 0;
    private float bufferDropTimeRemaining = 0;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps = 0;
    private int jumpCounter = 0;

    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;

    BoxCollider2D playerCollider;

    private TurtleEnemy ridingTurtleEnemy;

    private bool grounded = true;
    private bool isOnPlatform = false;
    public bool playerCanMove = true;
    private GameObject currentPlatform = null; 
    private Animator animator;
    private Rigidbody2D playerBody;
    private bool flightEnabled = false;
    private bool playerMoved = false;
    private bool playerKilled = false;
    // vars for platform falling
    private float dropTimer = 0f;
    public float dropDuration = 0.5f;

    public static PlayerMovement instance { get; private set; } // singleton player instance
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        playerBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerBody.freezeRotation = true;
        playerBody.gravityScale *= 2; 
    }
    private void Update()
    {
        float horizontalInput = getHorizontalInput();
        animator.SetBool("Running", horizontalInput != 0);
        animator.SetBool("Grounded", grounded);
        
        // Start timer and remove arrow key visuals
        if (!playerMoved && (
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D)))
        {
            playerMoved = true;
            if (UIManager.instance.timer != null) UIManager.instance.timer.StartTimer();
        }

        if (!playerCanMove)
        {
            grounded = true;
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x/1.01f, playerBody.linearVelocity.y);
            if (Input.GetKeyDown(KeyCode.Space)) // player beat the level, repurpose space bar to "Next Level"
            {
                if (playerKilled)
                { // reload same scene if player was killed
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                }
                else
                { // else level complete, move to next level
                    MenuSelector menuSelector = new MenuSelector();
                    menuSelector.SceneToLoad = "Next";
                    menuSelector.OpenScene();
                }
            }
            return;
        }
        if (playerCanMove)
        {
            playerBody.linearVelocity = new Vector2(horizontalInput * runSpeed, playerBody.linearVelocity.y);
        }
        if (playerBody.linearVelocity.y < -200) UIManager.instance.killed("You fell too fast and got dizzy");
        if (horizontalInput > 0f)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (horizontalInput < 0f)
        {
            transform.localScale = new Vector2(-1f,1f);
        }
        else // horizontal input is 0
        {
            if (playerCanMove)
            {
                playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x / 30f, playerBody.linearVelocity.y); // dampen horizontal movement when not pressing left/right
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (flightEnabled || grounded || coyoteCounter >= 0 || jumpCounter > 0) Jump();
            else
            {
                bufferJumpTimeRemaining = bufferTime;
            }
        }
        // allow short hops
        if ((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) && playerBody.linearVelocity.y > 0)
        {
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, playerBody.linearVelocity.y / 2f);
        }
        if (playerBody.linearVelocity.y > 35f && !(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            // Player is going up fast but not holding Up � apply damping
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, playerBody.linearVelocity.y * 0.8f);
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
            bufferJumpTimeRemaining -= Time.deltaTime;
            bufferDropTimeRemaining -= Time.deltaTime;
        }
        // platform falling
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (grounded)
            {
                Drop();
            }
            else
            {
                bufferDropTimeRemaining = bufferTime;
            }
        }

        if (playerBody.linearVelocity.y < -5) grounded = false;
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
        playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, jumpSpeed);
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

                    if (bufferJumpTimeRemaining > 0)
                    {
                        Jump();
                    }

                    if (bufferDropTimeRemaining > 0)
                    {
                        Drop();
                    }

                    break;
                }
            }
        }
        // logic for moving on enemies' backs
        var enemy = collision.collider.GetComponent<TurtleEnemy>();
        if (enemy != null)
        {
            ridingTurtleEnemy = enemy;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false;
        }
        if (collision.collider.GetComponent<TurtleEnemy>() == ridingTurtleEnemy)
        {
            ridingTurtleEnemy = null;
        }
    }
    public void killed()
    {
        playerKilled = true;
        playerCanMove = false;
    }
    private IEnumerator EnablePlayerMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        playerCanMove = true;
    }

    void Start()
    {
        playerCanMove = false;
        StartCoroutine(EnablePlayerMovementAfterDelay());
    }
    float getHorizontalInput()
    {
        if (playerCanMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) return -1f;
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) return 1f;
            else return 0f;
        }
        return playerBody.linearVelocity.x; // if player movement is disabled, just let the player continue moving
    }
    public void setGrounded(bool isGrounded)
    {
        grounded = isGrounded;
    }
    void FixedUpdate()
    {
        if (ridingTurtleEnemy != null)
        {
            playerBody.position += (Vector2)ridingTurtleEnemy.platformVelocity * Time.fixedDeltaTime;
        }
    }
}
