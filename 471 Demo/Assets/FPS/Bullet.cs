using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }
}
