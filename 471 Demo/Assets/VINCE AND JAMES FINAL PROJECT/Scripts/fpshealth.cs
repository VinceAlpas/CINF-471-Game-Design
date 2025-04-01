using UnityEngine;
using UnityEngine.SceneManagement;

public class fpshealth : MonoBehaviour
{
    public int health = 100;
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took damage! Health: " + health);

        if (cameraShake != null)
        {
            cameraShake.Shake(0.1f, 1.0f); 
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
    }
}
