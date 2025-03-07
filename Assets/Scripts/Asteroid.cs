using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float health = 4f;
    public bool isBigAsteroid;

    private void OnTriggerEnter2D(Collider2D collision) // Dùng trigger cho đạn
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= 1f;
            Destroy(collision.gameObject); // Phá hủy đạn
            if (health <= 0)
            {
                if (isBigAsteroid)
                {
                    GameManager.Instance.AddScore(100); // +100 cho asteroid lớn
                }
                else
                {
                    GameManager.Instance.AddScore(50); // +50 cho asteroid nhỏ
                }
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Deadzone"))
        {
                Destroy(gameObject);
        }
    }
}