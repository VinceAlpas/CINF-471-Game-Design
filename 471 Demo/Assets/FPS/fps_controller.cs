using UnityEngine;
using UnityEngine.InputSystem;

public class fps_controller : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float speed = 5f;
    [SerializeField] float mouseSensitivity = 100;
    [SerializeField] GameObject cam;

    [Header("Shooting Settings")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletSpawner; 

    Vector2 movement;
    Vector2 mouseMovement;
    CharacterController controller;
    float cameraUpRotation = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        // Debug: Confirm bulletSpawner and bullet are assigned at start
        if (bulletSpawner == null)
        {
            Debug.LogError("❌ BulletSpawner is MISSING at Start! Check Inspector.");
        }
        else
        {
            Debug.Log("✅ BulletSpawner is assigned correctly at Start.");
        }

        if (bullet == null)
        {
            Debug.LogWarning("⚠️ BulletPrefab was not set in the Inspector! Attempting to load from Resources.");
            bullet = Resources.Load<GameObject>("Bullet"); // Ensure the prefab is always accessible
        }

        if (bullet != null)
        {
            Debug.Log("✅ BulletPrefab successfully assigned.");
        }
        else
        {
            Debug.LogError("❌ BulletPrefab is STILL missing! Assign it in the Inspector.");
        }
    }

    void Update()
    {
        // Mouse Look
        float lookX = mouseMovement.x * Time.deltaTime * mouseSensitivity;
        float lookY = mouseMovement.y * Time.deltaTime * mouseSensitivity;

        cameraUpRotation -= lookY;
        cameraUpRotation = Mathf.Clamp(cameraUpRotation, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(cameraUpRotation, 0, 0);
        transform.Rotate(Vector3.up * lookX);

        // Player Movement
        float moveX = movement.x;
        float moveZ = movement.y;
        Vector3 moveDirection = (transform.right * moveX) + (transform.forward * moveZ);
        controller.SimpleMove(moveDirection * speed);

        // Failsafe: Auto-reassign BulletSpawner if lost
        if (bulletSpawner == null)
        {
            bulletSpawner = transform.Find("BulletSpawner")?.gameObject;
        }
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
        Debug.Log("🔫 OnAttack() called!");

        // Ensure BulletPrefab exists before shooting
        if (bullet == null)
        {
            Debug.LogError("❌ BulletPrefab is missing in OnAttack! Restoring reference...");
            bullet = Resources.Load<GameObject>("Bullet"); // Reload it just in case

            if (bullet == null)
            {
                Debug.LogError("❌ BulletPrefab is STILL missing! Assign it in the Inspector.");
                return;
            }
        }

        // Ensure BulletSpawner exists before shooting
        if (bulletSpawner == null)
        {
            Debug.LogError("❌ BulletSpawner is missing in OnAttack! Assign it in Inspector.");
            return;
        }

        // SHOOT THE BULLET
        Debug.Log("✅ Shooting Bullet!");
        Instantiate(bullet, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
    }
}
