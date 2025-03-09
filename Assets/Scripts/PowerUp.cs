using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Star,       // Tăng điểm
        UpgradeShot, // Nâng cấp đạn
        Shield,     // Lá chắn
        Invincibility // Bất tử + tăng tốc bắn
    }

    [SerializeField] private PowerUpType type;
    [SerializeField] private float fallSpeed = 1f; // Tốc độ rơi chậm
    [SerializeField] private int scoreValue = 200; // Điểm thưởng khi nhặt ngôi sao
    [SerializeField] private float duration = 5f; // Thời gian hiệu ứng (5s)

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.linearVelocity = new Vector2(0, -fallSpeed); // Rơi từ trên xuống với tốc độ chậm
        Destroy(gameObject, 10f); // Tự hủy sau 10s nếu không nhặt
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            ApplyPowerUp(player);
            Destroy(gameObject); // Hủy vật phẩm sau khi nhặt
        }
    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (type)
        {
            case PowerUpType.Star:
                GameManager.Instance.AddScore(scoreValue);
                Debug.Log("Collected Star! +200 points.");
                break;

            case PowerUpType.UpgradeShot:
                int currentShotLevel = player.GetCurrentShotLevel(); // Giả sử có phương thức này
                if (currentShotLevel < 3)
                {
                    player.UpgradeShot(); // Nâng cấp đạn
                    Debug.Log("Upgraded to Shot Level " + (currentShotLevel + 1));
                }
                else
                {
                    GameManager.Instance.AddScore(scoreValue * 2); // Thay vì lên cấp 4, cho điểm gấp đôi
                    Debug.Log("Max Shot Level reached! +100 points instead.");
                }
                break;

            case PowerUpType.Shield:
                player.ActivateShield(duration);
                Debug.Log("Shield activated for 5 seconds!");
                break;

            case PowerUpType.Invincibility:
                player.ActivateInvincibility(duration);
                Debug.Log("Invincibility and faster firing activated for 5 seconds!");
                break;
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPowerUpSFX(); // Âm thanh nhận power-up
        }
    }

    // Phương thức để gán sprite dựa trên type (sẽ được gọi từ Asteroid)
    public void SetPowerUpType(PowerUpType newType)
    {
        type = newType;
        // Ở đây bạn có thể gán sprite tương ứng (xử lý trong Unity Editor)
    }
}