using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 10;
    [SerializeField] float lifetime = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, lifetime); // Bullet destroys itself after a set time
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if it hits an enemy
        EnemyScript enemy = other.GetComponentInParent<EnemyScript>(); // Fixes nested colliders issue
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            Destroy(gameObject); // Destroy bullet on impact
        }
    }
}
