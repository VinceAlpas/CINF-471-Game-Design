using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Transform[] waypoints; // Patrol waypoints
    public float patrolSpeed = 3f; // Speed while patrolling
    public float chaseSpeed = 5f; // Speed while chasing
    public float chaseDistance = 5f; // Distance to start chasing
    public float attackDistance = 1.5f; // Distance to attack
    public GameObject bloodEffectPrefab; // Blood particle effect

    private int currentWaypointIndex = 0;
    private Rigidbody rb;
    private enum State { Patrolling, Chasing, Attacking }
    private State currentState = State.Patrolling;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError($"Rigidbody is missing on {gameObject.name}. Please add one.");
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log($"Player found: {player.name}");
            }
            else
            {
                Debug.LogError("Player not found! Make sure your Player is tagged as 'Player'.");
            }
        }

        if (waypoints.Length == 0)
        {
            Debug.LogError($"{gameObject.name} has no patrol waypoints assigned!");
        }
    }

    void FixedUpdate()
    {
        if (player == null) return; // Avoid errors if Player is not found

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
        {
            currentState = State.Attacking;
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Patrolling;
        }

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;
            case State.Chasing:
                ChasePlayer();
                break;
            case State.Attacking:
                AttackPlayer();
                break;
        }
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return; // Safety check

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Debug.Log(gameObject.name + " Moving to: " + targetWaypoint.name);

        MoveTowards(targetWaypoint.position, patrolSpeed);

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.3f) // If close to waypoint, go to the next one
        {
            Debug.Log($"Reached {targetWaypoint.name}, moving to next...");
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to next waypoint
        }
    }

    void ChasePlayer()
    {
        Debug.Log(gameObject.name + " Chasing Player!");
        MoveTowards(player.position, chaseSpeed);
    }

    void AttackPlayer()
    {
        Debug.Log(gameObject.name + " Attacking Player!");
        // Add attack animation or logic here
    }

    void MoveTowards(Vector3 targetPosition, float speed)
    {
        if (rb == null) return;

        // Calculate movement direction
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Apply movement without modifying y velocity
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // Rotate to face movement direction
        RotateTowards(targetPosition);
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;

        if (direction.magnitude > 0.1f)  // Prevent unnecessary rotations
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 200 * Time.deltaTime);
        }
    }

    public void TakeDamage()
    {
        if (bloodEffectPrefab != null)
        {
            Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
        }
        Debug.Log($"{gameObject.name} destroyed!");
        Destroy(gameObject);
    }
}
