using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Prefab of the enemy
    [SerializeField] private Transform spawnPoint; // Location where enemies will spawn
    [SerializeField] private float spawnInterval = 5f; // Time between enemy spawns

    void Start()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Enemy prefab or spawn point is not assigned in the Inspector.");
            return;
        }

        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Ensure there is an enemy prefab before instantiating
            if (enemyPrefab != null && spawnPoint != null)
            {
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                Debug.Log("Enemy spawned at: " + spawnPoint.position);
            }
            else
            {
                Debug.LogError("Cannot spawn enemy. Enemy prefab or spawn point is missing.");
            }
        }
    }
}
