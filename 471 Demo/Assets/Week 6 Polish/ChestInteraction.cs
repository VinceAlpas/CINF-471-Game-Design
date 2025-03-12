using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public Animator chestAnimator;
    public ParticleSystem chestParticles;
    public GameObject chestLock;
    public GameObject chestLoot;
    public AudioSource chestSound;
    public TextMeshProUGUI interactText;

    private bool isOpen = false;
    private bool playerNearby = false;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Interact.performed += ctx => TryOpenChest();
    }

    private void OnEnable() { inputActions.Enable(); }
    private void OnDisable() { inputActions.Disable(); }

    private void TryOpenChest()
    {
        if (playerNearby && !isOpen)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        if (chestAnimator != null)
        {
            Debug.Log("Triggering Chest Animation");
            chestAnimator.SetTrigger("OpenChest"); // Only play animation when pressing "E"
        }

        if (chestLock != null)
        {
            Debug.Log("Hiding Chest Lock");
            chestLock.SetActive(false);
        }

        if (chestParticles != null)
        {
            Debug.Log("Playing Chest Particles");
            chestParticles.Play();
        }

        if (chestLoot != null)
        {
            Debug.Log("Showing Chest Loot");
            chestLoot.SetActive(true);
        }

        if (chestSound != null)
        {
            Debug.Log("Playing Chest Sound");
            chestSound.Play();
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false); // Hide UI prompt after opening
        }

        isOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered chest area");
            playerNearby = true;
            if (!isOpen && interactText != null)
            {
                interactText.gameObject.SetActive(true); // Show UI prompt
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left chest area");
            playerNearby = false;
            if (interactText != null)
            {
                interactText.gameObject.SetActive(false); // Hide UI prompt
            }
        }
    }
}
