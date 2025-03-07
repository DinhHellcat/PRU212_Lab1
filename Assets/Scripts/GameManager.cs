using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton pattern để truy cập GameManager từ bất kỳ đâu
    public static GameManager Instance { get; private set; }

    private int score = 0; // Biến lưu điểm số
    public int Score
    {
        get { return score; }
    }

    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        // Đảm bảo chỉ có một GameManager trong scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ GameManager khi chuyển scene (nếu cần)
        }
        else
        {
            Destroy(gameObject); // Hủy nếu đã có GameManager khác
        }
    }
    private void Start()
    {
        UpdateScore();
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score updated! +" + points + " points. Total score: " + score);
        UpdateScore();
    }
    private void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
}