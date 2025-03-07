using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float firingCooldown;
    [SerializeField] private float health = 4f;
    private float tempCooldown;

    [SerializeField] private Sprite[] shipSprites; // Mảng 4 sprite tương ứng với 4 mức máu
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }
    void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        transform.Translate(direction * Time.deltaTime * moveSpeed);

        if (Input.GetKey(KeyCode.Space))
        {
            if (tempCooldown <= 0)
            {
                Fire();
                tempCooldown = firingCooldown;
            }
        }
        tempCooldown -= Time.deltaTime;
    }
    private void Fire()
    {
        ProjectileController p = Instantiate(projectile, firingPoint.position, Quaternion.identity, null);
        p.handleProjectile();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            health -= 1f; // Giảm máu người chơi khi bị thiên thạch đâm
            Debug.Log("Player health: " + health);
            Destroy(collision.gameObject);
            UpdateSprite();
            if (health <= 0)
            {
                Debug.Log("Player destroyed!");
                Destroy(gameObject); // Phá hủy người chơi khi hết máu
            }
        }
    }

    private void UpdateSprite()
    {
        // Chuyển mức máu thành chỉ số của sprite (0, 1, 2, 3)
        int spriteIndex = Mathf.FloorToInt(health); // Làm tròn xuống
        if (spriteIndex >= 0 && spriteIndex < shipSprites.Length)
        {
            spriteRenderer.sprite = shipSprites[spriteIndex];
        }
    }
}
