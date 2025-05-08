using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Collectable : MonoBehaviour
{
    [SerializeField] private int eggNumber;
    private void Reset()
    {
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIManager.instance.EggCollected(eggNumber);
        Destroy(gameObject);
    }
}
