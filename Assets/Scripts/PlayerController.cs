using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float firingCooldown;
    [SerializeField] private float health = 4f;
    private float tempCooldown;

    [SerializeField] private Sprite[] shipSprites;// Mảng sprite của tàu
    [SerializeField] private Sprite[] heartSprites;// Mảng sprite của trái tim (4 trạng thái)
    [SerializeField] private Image heartImage; // Mảng 4 sprite tương ứng với 4 mức máu
    private SpriteRenderer spriteRenderer;

    public Vector2 minBounds = new Vector2(-5f, -5f); // Giới hạn dưới-trái
    public Vector2 maxBounds = new Vector2(5f, 5f);
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
        UpdateHeartUI();

    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        //transform.Translate(direction * Time.deltaTime * moveSpeed);

        Vector2 moveStep = direction * Time.deltaTime * moveSpeed;

        Vector2 newPosition = (Vector2)transform.position + moveStep;

        // Giới hạn vị trí trong Bounds
        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        // Di chuyển Player tới vị trí đã giới hạn
        transform.position = newPosition;


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
            UpdateHeartUI();
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
    private void UpdateHeartUI()
    {
        int heartIndex = Mathf.FloorToInt(health);
        if( heartIndex >= 0 && heartIndex < heartSprites.Length)
        {
            heartImage.sprite = heartSprites[heartIndex];
        }
    }
}
