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
    public bool sendPlayerSideways = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<Collider2D>().enabled = !disableCollider;
        Rigidbody2D playerBody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (sendPlayerSideways)
        {
            StartCoroutine(launchSideways(playerBody));
        }
        else
        {
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, reboundSpeed * 35);
        }
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
    private IEnumerator launchSideways(Rigidbody2D playerBody)
    {
        GetComponent<Collider2D>().enabled = false;
        PlayerMovement.instance.playerCanMove = false;
        PlayerMovement.instance.setGrounded(false);
        playerBody.linearVelocity = new Vector2(-250f * reboundSpeed, 35f);
        yield return new WaitForSeconds(0.4f);
        PlayerMovement.instance.playerCanMove = true;
        GetComponent<Collider2D>().enabled = true;
    }
}