using UnityEngine;

public class LavaDie : MonoBehaviour
{
    public Transform respawnPoint; // Assign this in the Inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            // Reset position
            other.transform.position = respawnPoint.position;

            // Reset Rigidbody physics
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Stop movement
                rb.angularVelocity = Vector3.zero; // Stop rotation/spinning
            }

            Debug.Log("Player fell into lava! Respawning and resetting physics...");
        }
    }
}
