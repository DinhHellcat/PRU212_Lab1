using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float health = 4f;

    private void OnTriggerEnter2D(Collider2D collision) // Dùng trigger cho đạn
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= 1f;
            Destroy(collision.gameObject); // Phá hủy đạn
            if (health <= 0)
            {
                Debug.Log("Asteroid destroyed!");
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Deadzone"))
        {
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Giữ va chạm vật lý cho Player
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Asteroid hit Player!");
        }
    }
}