using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public GameObject bloodEffectPrefab;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Get reference to GameManager
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit!");

            if (bloodEffectPrefab != null)
            {
                Instantiate(bloodEffectPrefab, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject);

            if (gameManager != null)
            {
                gameManager.EnemyDefeated(); // Notify GameManager that an enemy is eliminated
            }
        }
    }
}
