using UnityEngine;
using UnityEngine.InputSystem;

public class Quiz : MonoBehaviour
{

    public float speed = 0.5f;
    public GameObject friend;

    Vector2 movement;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject friend = GameObject.Find("Billy");
        rb = GetComponent<Rigidbody>();
        print("Hi " + friend.name + "!");
        print("I think I've passed the quiz!");
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = movement.x;
        float moveZ = movement.y;

        Vector3 force = new Vector3(moveX, 0, moveZ);
        rb.AddForce(force * speed);
    }

    void OnMove(InputValue moveVal) 
    {
        movement = moveVal.Get<Vector2>();
    }
}
