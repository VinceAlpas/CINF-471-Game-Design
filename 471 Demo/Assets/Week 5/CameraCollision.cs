using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform player; // The player character
    public float cameraDistance = 5f; // Default camera distance
    public float minDistance = 1f; // How close the camera can get to the player
    public float smoothSpeed = 10f; // How smooth the transition is
    public LayerMask collisionMask; // Walls and objects that should block the camera

    void LateUpdate()
    {
        Vector3 direction = transform.position - player.position; // Direction from player to camera
        float targetDistance = cameraDistance;

        // Raycast from the player to the camera
        if (Physics.Raycast(player.position, direction.normalized, out RaycastHit hit, cameraDistance, collisionMask))
        {
            targetDistance = Mathf.Clamp(hit.distance - 0.2f, minDistance, cameraDistance); // Adjust distance
        }

        // Smoothly move camera to the new position
        Vector3 newPosition = player.position + direction.normalized * targetDistance;
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);
    }
}
