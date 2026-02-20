using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    public string causeOfDeath = "You got sleepy and closed your eyes";
    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit Enemy");
            Physics.IgnoreCollision(collision.GetComponent<Collider>(), GetComponent<Collider>());
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.instance.killed(causeOfDeath);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic; // or use constraints below
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            // Make it invisible
            SpriteRenderer sr = collision.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = false;
            }

            // Disable animations if any
            Animator animator = collision.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }

            // Optionally disable collider
            Collider2D col = collision.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}
