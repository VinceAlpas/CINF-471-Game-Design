using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int health = 5;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took damage! Remaining Health: " + health);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
