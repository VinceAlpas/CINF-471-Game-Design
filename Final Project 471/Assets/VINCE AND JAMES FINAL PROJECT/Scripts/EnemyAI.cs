using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Pace,
        Follow,
        Attack  // New state for when the enemy attacks
    }

    [SerializeField]
    GameObject[] route;
    public GameObject target;
    int routeIndex = 0;

    [SerializeField]
    float speed = 3.0f;
    public int enemyHealth = 100;
    public GameObject deathParticlesPrefab;

    public State currentState = State.Pace;
    private float attackRange = 2.0f;  // Distance at which the enemy will attack the player
    private float attackCooldown = 2.0f;  // Time between attacks
    private float lastAttackTime = 0f;

    void Start() {}

    void Update()
    {
        switch (currentState)
        {
            case State.Pace:
                OnPace();
                break;
            case State.Follow:
                OnFollow();
                break;
            case State.Attack:
                OnAttack();
                break;
        }

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void OnPace()
    {
        target = route[routeIndex];
        MoveTo(target);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            routeIndex += 1;
            if (routeIndex >= route.Length) routeIndex = 0;
        }

        GameObject obstacle = CheckForward();
        if (obstacle != null && obstacle.CompareTag("Player"))
        {
            target = obstacle;
            currentState = State.Follow;
        }
    }

    void OnFollow()
    {
        if (target == null) return;

        PlayerStateManager playerState = target.GetComponent<PlayerStateManager>();
        if (playerState != null && playerState.isSneaking)
        {
            currentState = State.Pace; // Switch back to pacing if sneaking
            return;
        }

        MoveTo(target);

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget < attackRange)
        {
            StartCoroutine(StartAttack());
        }

        GameObject obstacle = CheckForward();
        if (obstacle != null && obstacle.CompareTag("Player"))
        {
            target = obstacle;
            currentState = State.Follow;
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.2f);  // Small delay to avoid flickering
        currentState = State.Attack;
    }

    void OnAttack()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget >= attackRange + 1.0f)
        {
            currentState = State.Follow;
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            fpshealth playerHealth = target.GetComponent<fpshealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                lastAttackTime = Time.time;
                Debug.Log("Enemy attacked the player!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Bullets bullet = other.GetComponent<Bullets>();
        if (bullet != null)
        {
            TakeDamage(10);
            Destroy(other.gameObject);
            Debug.Log("Enemy hit by a bullet!");
            return;
        }

        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            currentState = State.Follow;
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        Debug.Log("Enemy took damage. Remaining health: " + enemyHealth);

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 Enemy died!");
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }
        
        currentState = State.Pace;
        enabled = false;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

        Collider enemyCollider = GetComponent<Collider>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
    }

    void MoveTo(GameObject t)
    {
        transform.position = Vector3.MoveTowards(transform.position, t.transform.position, speed * Time.deltaTime);
        Vector3 directionToTarget = t.transform.position - transform.position;
        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    GameObject CheckForward()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            PlayerStateManager player = hit.transform.gameObject.GetComponent<PlayerStateManager>();
            if (player != null && !player.isSneaking)
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}