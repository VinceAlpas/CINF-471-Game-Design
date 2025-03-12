using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject bloodEffectPrefab; // Assign BloodEffect prefab in Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if we hit an enemy
        {
            Debug.Log("Enemy Hit!");

            // Spawn blood effect at enemy position
            if (bloodEffectPrefab != null)
            {
                Instantiate(bloodEffectPrefab, other.transform.position, Quaternion.identity);
            }

            // Destroy the enemy
            Destroy(other.gameObject);
        }
    }
}
