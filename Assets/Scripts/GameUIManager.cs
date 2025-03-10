using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI scoreText; // ScoreText trong game
    [SerializeField] private TextMeshProUGUI gameOverScoreText; // ScoreText trong GameOverCanvas
    [SerializeField] private TextMeshProUGUI timeSurvivedText;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    private bool isPaused = false;

    void Start()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        UpdateScoreDisplay(); // Cập nhật điểm ban đầu

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameBGMusic();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverCanvas.activeSelf)
        {
            TogglePause();
        }
        UpdateScoreDisplay(); // Cập nhật điểm liên tục
    }

    public void TogglePause()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ShowGameOver()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGMusic(); // Tắt nhạc nền khi game over
        }
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);

        // Hiển thị điểm trong Game Over
        gameOverScoreText.text = "Score: " + GameManager.Instance.Score.ToString();

        // Hiển thị thời gian sinh tồn
        float difficultyTimer = asteroidSpawner.GetDifficultyTimer();
        float minutes = Mathf.FloorToInt(difficultyTimer / 60f);
        float seconds = Mathf.FloorToInt(difficultyTimer % 60f);
        timeSurvivedText.text = "Time Survived: " + string.Format("{0:00}:{1:00}", minutes, seconds);

        // Lưu dữ liệu vào PlayerPrefs
        GameManager.Instance.SaveGameData(difficultyTimer);
    }

    // Cập nhật hiển thị điểm số
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.Score.ToString();
        }
    }
}