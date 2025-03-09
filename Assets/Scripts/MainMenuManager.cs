using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private TextMeshProUGUI[] scoreEntries;
    [SerializeField] private GameObject clearDataButton;

    void Start()
    {
        highScorePanel.SetActive(false);
        DisplayHighScores();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuBGMusic();
        }
    }

    public void PlayGame()
    {
        // Reset score khi bắt đầu một lần chơi mới
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetScore(); // Đặt lại điểm về 0
            Debug.Log("Score reset to 0 before starting new game.");
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
        Application.Quit();
        Debug.Log("Game Quit");
    }

    private void DisplayHighScores()
    {
        int count = PlayerPrefs.GetInt("HighScoreCount", 0);
        for (int i = 0; i < scoreEntries.Length; i++)
        {
            if (i < count)
            {
                string score = PlayerPrefs.GetString("Score" + i, "0");
                string time = PlayerPrefs.GetString("Time" + i, "00:00");
                scoreEntries[i].text = (i + 1) + " --- " + score + " --- " + time;
            }
            else
            {
                scoreEntries[i].text = (i + 1) + " --- 0 --- 00:00";
            }
        }
    }

    public void ClearData()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
        PlayerPrefs.DeleteAll();
        DisplayHighScores();
        Debug.Log("All game data cleared!");
    }

    public void ToggleHighScorePanel()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX(); // Âm thanh nhấn nút
        }
        bool isActive = highScorePanel.activeSelf;
        highScorePanel.SetActive(!isActive);
    }
}