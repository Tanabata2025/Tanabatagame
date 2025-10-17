using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 👇 この public が重要！
    public GameObject gameOverUI;

    private bool isGameOver = false;
    private float gameOverLineY;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        float bottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        float top = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, -Camera.main.transform.position.z)).y;
        float height = top - bottom;
        gameOverLineY = bottom + height / 8f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void CheckGameOver(float enemyY)
    {
        if (!isGameOver && enemyY <= gameOverLineY)
        {
            GameOver();
        }
    }

    void Update()
    {
        // Rキーでリトライ
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GameOver()
    {
        isGameOver = true;
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }
}
