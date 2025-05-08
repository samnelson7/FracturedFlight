using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BranchCollision : MonoBehaviour
{
    [SerializeField] private bool disableCollider = false;
    [SerializeField] private bool continueSpinning = false;
    [SerializeField] private int maxDegreesOfRotation = 180;
    [SerializeField] private float rotationSpeed = 90f;
    private bool objectHit = false;
    private float degreesRotated = 0;
    public int rotationDirection = 1;
    public float reboundSpeed = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Collider2D>().enabled = !disableCollider;
        Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
        playerBody.velocity = new Vector2(playerBody.velocity.x, reboundSpeed*35);
        objectHit = true;        
    }
    private void Update()
    {
        if ((objectHit && degreesRotated < maxDegreesOfRotation) || continueSpinning)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime * rotationDirection * -1f;
            transform.parent.Rotate(0, 0, rotationThisFrame);
            degreesRotated += Mathf.Abs(rotationThisFrame);
        }
        if (degreesRotated >= maxDegreesOfRotation && !continueSpinning)
        {
            degreesRotated = 0;
            objectHit = false;
        }
    }
}