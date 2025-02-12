using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int health = 5;
    [SerializeField] float speed = 1f;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float sightRange = 8f;
    [SerializeField] float chargeDistance = 4f;
    [SerializeField] float attackRange = 1.5f;
    private float damageCooldown = 3f;

    private Transform player;
    private PlayerHealth playerHealth;
    private Rigidbody rb;
    private bool isCharging = false;
    private bool canDamagePlayer = true;

    [SerializeField] GameObject enemyPrefab; // Prefab for respawning
    [SerializeField] Transform spawnPoint;   // Fixed spawn location (Set in Inspector)
    [SerializeField] float spawnHeightOffset = 2f; // Adjusts spawn height above the ground

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (spawnPoint == null)
        {
            Debug.LogError("❌ ERROR: SpawnPoint is not assigned! Assign it in the Inspector.");
            return;
        }

        // Set the initial position from a fixed spawn point
        transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeightOffset, spawnPoint.position.z);
        transform.rotation = spawnPoint.rotation;

        InvokeRepeating(nameof(FindPlayer), 0f, 2f); // Check for the player every 2 seconds
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chargeDistance)
        {
            ChargeAtPlayer();
        }
        else if (distanceToPlayer <= sightRange)
        {
            FollowPlayer();
        }
    }

    void FindPlayer()
    {
        if (player != null) return;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError("❌ ERROR: Player exists but does NOT have a PlayerHealth component!");
            }
            else
            {
                Debug.Log("✅ Enemy successfully found the Player!");
            }
        }
        else
        {
            Debug.LogError("❌ ERROR: Player reference is NULL! Retrying...");
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        isCharging = false;
        Vector3 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
    }

    void ChargeAtPlayer()
    {
        if (!isCharging && player != null)
        {
            isCharging = true;
            Vector3 chargeDirection = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector3(chargeDirection.x * chargeSpeed, rb.linearVelocity.y, chargeDirection.z * chargeSpeed);
        }
    }

    void AttackPlayer()
    {
        if (!canDamagePlayer) return;

        if (player == null)
        {
            Debug.LogError("❌ ERROR: Player reference is NULL during attack! Retrying...");
            FindPlayer();
            return;
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20);
            Debug.Log($"💥 Player took 20 damage! Health left: {playerHealth.health}");
            StartCoroutine(DamageCooldown());
        }
        else
        {
            Debug.LogError("❌ ERROR: PlayerHealth component is NULL during attack!");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"💥 Enemy took {damage} damage! Remaining Health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("☠️ Enemy has been defeated!");
        StartCoroutine(RespawnEnemy());
        gameObject.SetActive(false);
    }

    IEnumerator RespawnEnemy()
    {
        Debug.Log("🔄 Respawning enemy in 5 seconds...");
        yield return new WaitForSeconds(5f);

        if (enemyPrefab != null && spawnPoint != null)
        {
            Vector3 adjustedSpawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeightOffset, spawnPoint.position.z);
            GameObject newEnemy = Instantiate(enemyPrefab, adjustedSpawnPosition, spawnPoint.rotation);
            newEnemy.GetComponent<EnemyScript>().FindPlayer();
            Debug.Log($"✅ Enemy has respawned at: {adjustedSpawnPosition}");
        }
        else
        {
            Debug.LogError("❌ ERROR: Enemy prefab or spawn point is missing! Assign them in the Inspector.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.contacts[0].point.y > transform.position.y + 0.5f)
            {
                Die();
            }
            else
            {
                AttackPlayer();
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        canDamagePlayer = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamagePlayer = true;
    }
}
