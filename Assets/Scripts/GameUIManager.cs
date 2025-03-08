using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas; // Canvas chứa Pause Menu
    [SerializeField] private GameObject gameOverCanvas; // Canvas chứa Game Over
    [SerializeField] private TextMeshProUGUI scoreText; // Hiển thị điểm trong Game Over
    [SerializeField] private TextMeshProUGUI timeSurvivedText; // Hiển thị thời gian sinh tồn
    [SerializeField] private AsteroidSpawner asteroidSpawner; // Để lấy thời gian

    private bool isPaused = false;

    void Start()
    {
        // Đảm bảo cả hai Canvas ẩn khi bắt đầu
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Tạm dừng game khi nhấn phím Escape
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverCanvas.activeSelf)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ContinueGame()
    {
        TogglePause();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Hiển thị Game Over Canvas khi người chơi thua
    public void ShowGameOver()
    {
        Time.timeScale = 0f; // Tạm dừng game
        gameOverCanvas.SetActive(true);

        // Hiển thị điểm
        scoreText.text = "Score: " + GameManager.Instance.Score.ToString();

        // Hiển thị thời gian sinh tồn
        float difficultyTimer = asteroidSpawner.GetDifficultyTimer();
        float minutes = Mathf.FloorToInt(difficultyTimer / 60f);
        float seconds = Mathf.FloorToInt(difficultyTimer % 60f);
        timeSurvivedText.text = "Time Survived: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}