using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 direction = Vector2.up;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * moveSpeed;
        handleProjectile();
    }

    public void handleProjectile()
    {
        Destroy(gameObject, 2f);
    }
}