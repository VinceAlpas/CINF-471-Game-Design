using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f; // Adjust for faster/slower rotation
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock cursor
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera vertically (up and down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotating

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player horizontally (left and right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
