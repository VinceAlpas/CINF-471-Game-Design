using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed; // Corrected from linearVelocity to velocity
    }
}

