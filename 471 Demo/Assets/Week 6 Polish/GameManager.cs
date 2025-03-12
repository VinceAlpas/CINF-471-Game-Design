using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene reloading

public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    private int enemiesRemaining;

    void Start()
    {
        Debug.Log("GameManager is active!");
        enemiesRemaining = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Enemies at start: " + enemiesRemaining);

        // Hide cursor at start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        Debug.Log("Enemy Defeated! Remaining: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            ShowWinScreen();
        }
    }

    void ShowWinScreen()
    {
        Debug.Log("Win Screen should appear!");

        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Debug.Log("Win Screen ACTIVATED!");

            // Show mouse cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("WinScreen Panel is NOT assigned in GameManager!");
        }
    }

    // 🔹 Ensure RestartLevel() is called
    public void RestartLevel()
    {
        Debug.Log("Restart Level button clicked!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
