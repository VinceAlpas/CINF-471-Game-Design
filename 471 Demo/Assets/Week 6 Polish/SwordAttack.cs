using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if we hit an enemy
        {
            Debug.Log("Enemy Hit!");
            Destroy(other.gameObject); // Destroy the enemy
        }
    }
}
