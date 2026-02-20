using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public float spawnInterval = 2f; // Time interval between spawns

    void Start()
    {
        // Start the spawn routine
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Debug.Log("Spawning enemy");
            // Instantiate the enemy at the spawn location
            Instantiate(enemyPrefab, transform.position, transform.rotation);

            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
