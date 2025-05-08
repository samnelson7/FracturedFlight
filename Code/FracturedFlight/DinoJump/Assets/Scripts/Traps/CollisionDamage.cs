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
        UIManager.instance.killed(causeOfDeath);
        Destroy(collision.gameObject);
    }
}
