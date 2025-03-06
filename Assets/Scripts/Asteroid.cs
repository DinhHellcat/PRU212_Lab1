using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // Phá hủy đạn
            Destroy(gameObject);           // Phá hủy thiên thạch
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Xử lý khi người chơi bị thiên thạch đâm
            Debug.Log("Player hit by asteroid!");
            Destroy(gameObject); // Phá hủy thiên thạch
            // Thêm logic tổn thương người chơi ở đây (ví dụ: giảm máu)
        }
    }
}