using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Gọi khi nhấn nút Play
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); // Tải Scene game (đặt tên Scene game của bạn là "Game")
    }

    // Gọi khi nhấn nút Quit
    public void QuitGame()
    {
        Application.Quit(); // Thoát chương trình
        Debug.Log("Game Quit"); // Debug để kiểm tra trong Editor
    }
}