using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float health = 4f; // Máu của thiên thạch

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= 1f; // Giảm máu thiên thạch khi trúng đạn
            Destroy(collision.gameObject); // Phá hủy đạn
            Debug.Log("Projectile destroyed");
            if (health <= 0)
            {
                Debug.Log("Asteroid destroyed!");
                Destroy(gameObject); // Phá hủy thiên thạch khi hết máu
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Asteroid hit Player!");
        }
    }
}