using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;
    private bool isOpen = false;
    private bool playerNearby = false;

    void Update()
    {
        // Check if player is nearby and presses E
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        chestAnimator.SetTrigger("Open"); // Play animation
        isOpen = true;
    }

    // Detect when player is near
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    // Detect when player leaves
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
