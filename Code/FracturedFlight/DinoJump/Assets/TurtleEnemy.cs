using System.Collections;
using UnityEngine;

public class TurtleEnemy : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;

    [Header("Spike Settings")]
    [SerializeField] private float spikeOnDuration = 2f;
    [SerializeField] private float spikeOffDuration = 3f;
    [SerializeField] private bool cycleSpikes = false;
    [SerializeField] private string causeOfDeath = "The spiny startled you";

    public Vector3 platformVelocity { get; private set; }

    private Vector3 currentTarget;
    private Vector3 lastPosition;
    private Animator animator;
    private bool spikesOut = false;
    private void Start()
    {
        currentTarget = pointB.position;
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        if(cycleSpikes) StartCoroutine(SpikeCycle());
    }

    private void Update()
    {
        // Create target position using only x from currentTarget, and current y from transform.position
        Vector3 targetPosition = new Vector3(currentTarget.x, transform.position.y, transform.position.z);

        // Move only in x direction
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Update platform velocity
        platformVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // Switch direction if close to target in x only
        if (Mathf.Abs(transform.position.x - currentTarget.x) < 0.05f)
        {
            SwitchTarget();
            FlipSprite();
        }
    }

    private void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA.position) ? pointB.position : pointA.position;
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only kill player if spikes are out
        if (spikesOut && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            Animator playerAnim = collision.gameObject.GetComponent<Animator>();
            if (playerAnim != null) playerAnim.enabled = false;

            Collider2D col = collision.gameObject.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
            UIManager.instance.killed(causeOfDeath);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Only kill player if spikes are out
        if (spikesOut && collision.gameObject.CompareTag("Player"))
        {
            UIManager.instance.killed(causeOfDeath);

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;

            Animator playerAnim = collision.gameObject.GetComponent<Animator>();
            if (playerAnim != null) playerAnim.enabled = false;

            Collider2D col = collision.gameObject.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }
    }
    private IEnumerator SpikeCycle()
    {
        while (true)
        {
            // Turn spikes on
            animator.SetBool("Spikes", true);
            yield return new WaitForSeconds(0.8f);
            spikesOut = true;
            yield return new WaitForSeconds(spikeOnDuration);

            // Turn spikes off
            animator.SetBool("Spikes", false);
            yield return new WaitForSeconds(0.8f);
            spikesOut = false;
            yield return new WaitForSeconds(spikeOffDuration);
        }
    }
}
