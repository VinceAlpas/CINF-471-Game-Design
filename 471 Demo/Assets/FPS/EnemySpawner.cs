using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // Drag the Enemy Prefab here in Inspector
    [SerializeField] Transform spawnPoint;  // Drag an empty GameObject here (where enemy should appear)

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("❌ EnemyPrefab is missing in EnemySpawner! Assign it in the Inspector.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("❌ SpawnPoint is missing in EnemySpawner! Assign an empty GameObject.");
            return;
        }

        // ✅ Spawn the enemy at game start
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("✅ Enemy Spawned at: " + spawnPoint.position);
    }
}
