using UnityEngine;
using UnityEngine.InputSystem;

public class fps_controller : MonoBehaviour
{
    Vector2 movement;
    Vector2 mouseMovement;
    float cameraUpRotation = 0;
    CharacterController controller;
    
    [SerializeField] float speed = 2.0f;
    [SerializeField] float mouseSensitivity = 100;
    [SerializeField] GameObject cam;
    [SerializeField] GameObject bulletSpawner;
    [SerializeField] GameObject bullet;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float lookX = mouseMovement.x * Time.deltaTime * mouseSensitivity;
        float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

        cameraUpRotation -= lookY;
        cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0);
        transform.Rotate(Vector3.up * lookX);

        float moveX = movement.x;
        float moveZ = movement.y;

        Vector3 actual_movement = (transform.forward * moveZ) + (transform.right * moveX);
        controller.Move(actual_movement * Time.deltaTime * speed);
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    void OnLook(InputValue lookVal)
    {
        mouseMovement = lookVal.Get<Vector2>();
    }

    void OnAttack(InputValue attackVal)
    {
        Instantiate(bullet, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
    }
}
