using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 direction = Vector2.up; // Mặc định đạn bắn lên
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * moveSpeed; // Gán vận tốc cho đạn
        handleProjectile();
    }

    public void handleProjectile()
    {
        Destroy(gameObject, 2f); // Phá hủy đạn sau 2 giây nếu không trúng gì
    }
}