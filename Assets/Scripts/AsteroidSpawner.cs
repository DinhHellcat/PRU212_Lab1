using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab; // Prefab của thiên thạch
    [SerializeField] private Transform[] spawnPoints;   // Mảng 5 vị trí spawn
    [SerializeField] private Transform centerPoint;     // Vị trí trung tâm
    [SerializeField] private Vector2 rectangleSize = new Vector2(4f, 2f); // Kích thước hình chữ nhật (rộng, cao)
    [SerializeField] private float spawnInterval = 2f;  // Thời gian giữa các lần spawn
    [SerializeField] private Sprite[] bigAsteroidSprites;  // Sprite cho thiên thạch lớn
    [SerializeField] private Sprite[] smallAsteroidSprites;// Sprite cho thiên thạch nhỏ
    [SerializeField] private float bigAsteroidSpeed = 2f;  // Tốc độ của thiên thạch lớn
    [SerializeField] private float smallAsteroidSpeed = 3f;// Tốc độ của thiên thạch nhỏ

    private float timer;

    void Start()
    {
        if (spawnPoints.Length != 5)
        {
            Debug.LogError("Hãy gán đúng 5 spawn points!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnAsteroid();
            timer = 0f;
        }
    }

    void SpawnAsteroid()
    {
        // Chọn ngẫu nhiên một spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPosition = spawnPoints[spawnIndex].position;

        // Tạo vị trí ngẫu nhiên trong hình chữ nhật quanh centerPoint
        Vector2 randomTarget = GetRandomPointInRectangle();

        // Tạo thiên thạch
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Random loại thiên thạch (to hoặc nhỏ)
        bool isBig = Random.value > 0.5f;
        Sprite[] spriteArray = isBig ? bigAsteroidSprites : smallAsteroidSprites;

        // Random sprite từ mảng
        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteArray[Random.Range(0, spriteArray.Length)];

        // Random xoay sprite
        float randomRotation = Random.Range(0f, 360f);
        asteroid.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        // Tính hướng di chuyển từ spawn point đến target
        Vector2 direction = (randomTarget - spawnPosition).normalized;

        // Gán vận tốc cho thiên thạch
        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        float speed = isBig ? bigAsteroidSpeed : smallAsteroidSpeed; // Chọn tốc độ dựa trên loại
        rb.linearVelocity = direction * speed;
    }

    Vector2 GetRandomPointInRectangle()
    {
        // Tạo một điểm ngẫu nhiên trong hình chữ nhật
        float halfWidth = rectangleSize.x / 2f;
        float halfHeight = rectangleSize.y / 2f;
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);
        return (Vector2)centerPoint.position + new Vector2(randomX, randomY);
    }

    // Vẽ vùng hình chữ nhật trong Editor để dễ debug
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPoint.position, rectangleSize);
    }

    // Hàm để tăng tốc độ thiên thạch (dùng sau này)
    public void IncreaseSpeed(float increment)
    {
        bigAsteroidSpeed += increment;
        smallAsteroidSpeed += increment;
    }
}