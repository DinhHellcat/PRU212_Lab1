using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float health = 4f;
    public bool isBigAsteroid;
    [SerializeField] private GameObject[] powerUpPrefabs; // Mảng prefab của các vật phẩm

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            health -= collision.GetComponent<ProjectileController>().GetDamage(); // Lấy sát thương từ đạn
            Destroy(collision.gameObject);
            if (health <= 0)
            {
                if (isBigAsteroid)
                {
                    GameManager.Instance.AddScore(100);
                    SpawnPowerUp(0.3f); // 30% xác suất rớt vật phẩm với thiên thạch lớn
                }
                else
                {
                    GameManager.Instance.AddScore(50);
                    SpawnPowerUp(0.2f); // 20% xác suất rớt vật phẩm với thiên thạch nhỏ
                }
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Deadzone"))
        {
            Destroy(gameObject);
        }
    }

    private void SpawnPowerUp(float dropChance)
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length < 4)
        {
            Debug.LogWarning("PowerUpPrefabs is not properly set or has insufficient elements! Current length: " + (powerUpPrefabs?.Length ?? 0));
            return;
        }

        if (Random.value <= dropChance)
        {
            int powerUpIndex = GetRandomPowerUpIndex();
            Debug.Log("Spawning power-up at index: " + powerUpIndex);
            GameObject powerUp = Instantiate(powerUpPrefabs[powerUpIndex], transform.position, Quaternion.identity);
            PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();
            if (powerUpScript != null)
            {
                powerUpScript.SetPowerUpType((PowerUp.PowerUpType)powerUpIndex);
            }
            else
            {
                Debug.LogError("PowerUp script not found on prefab at index " + powerUpIndex);
            }
        }
    }

    private int GetRandomPowerUpIndex()
    {
        float roll = Random.value;
        if (roll < 0.5f) return (int)PowerUp.PowerUpType.Star; // 50% Ngôi sao
        else if (roll < 0.75f) return (int)PowerUp.PowerUpType.Shield; // 25% Lá chắn
        else if (roll < 0.9f) return (int)PowerUp.PowerUpType.UpgradeShot; // 15% Nâng cấp đạn
        else return (int)PowerUp.PowerUpType.Invincibility; // 10% Bất tử
    }
}