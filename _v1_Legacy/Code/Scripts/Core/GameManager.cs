using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Data")]
    public float PlayerXP;
    // public List<Item> Inventory

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ToMainMenu()
    {
        Debug.Log("Going back to main menu... once it will be available....");
    }

    public void QuitGame()
    {
        Debug.Log("Exit game, nothing to see here!");
        Application.Quit();
    }

    public void OnPlayerDeath()
    {
        Debug.Log("GAME OVER! This is a game over screen");
        Time.timeScale = 0.0f;
        UIManager.Instance.ShowGameOver();
    }

    public void LoadNextLevel()
    {
        Debug.Log("Level Complete! loading next level");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void RestartLevel()
    {
        Debug.Log("Restarting Level");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
