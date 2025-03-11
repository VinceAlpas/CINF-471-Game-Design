using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;
    public ParticleSystem chestParticles;
    private bool isOpen = false;
    private bool playerNearby = false;

    private InputSystem_Actions inputActions; // Match the generated class name

    private void Awake()
    {
        inputActions = new InputSystem_Actions(); // Use the correct class
        inputActions.Player.Interact.performed += ctx => TryOpenChest();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void TryOpenChest()
    {
        if (playerNearby && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        chestAnimator.SetTrigger("OpenChest"); // Trigger the animation

        if (chestParticles != null)
        {
            chestParticles.Play(); // Play particle effect
        }

        isOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
