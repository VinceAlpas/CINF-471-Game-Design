using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject bloodEffectPrefab; // Assign in Inspector

    public void TakeDamage()
    {
        // Spawn blood effect at enemy position
        if (bloodEffectPrefab != null)
        {
            Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
        }

        // Destroy enemy
        Destroy(gameObject);
    }
}
