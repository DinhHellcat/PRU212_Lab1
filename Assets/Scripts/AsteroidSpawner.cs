using UnityEngine;
using TMPro; // Thêm namespace cho TextMeshPro

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Vector2 rectangleSize = new Vector2(4f, 2f);
    [SerializeField] private float initialSpawnInterval = 0.7f; // Khởi đầu
    [SerializeField] private float initialBigAsteroidSpeed = 3f; // Đổi: thiên thạch lớn nhanh hơn
    [SerializeField] private float initialSmallAsteroidSpeed = 2f; // Đổi: thiên thạch nhỏ chậm hơn
    [SerializeField] private Sprite[] bigAsteroidSprites;
    [SerializeField] private Sprite[] smallAsteroidSprites;

    // Giá trị sau 15 giây
    private float secondStageSpawnInterval = 0.5f;
    private float secondStageBigAsteroidSpeed = 4f; // Đổi: thiên thạch lớn
    private float secondStageSmallAsteroidSpeed = 3f; // Đổi: thiên thạch nhỏ

    // Giá trị tối đa (phút thứ 2)
    private float maxSpawnInterval = 0.2f;
    private float maxBigAsteroidSpeed = 8f; // Đổi: thiên thạch lớn
    private float maxSmallAsteroidSpeed = 6f; // Đổi: thiên thạch nhỏ

    private float currentSpawnInterval;
    private float currentBigAsteroidSpeed;
    private float currentSmallAsteroidSpeed;
    private float currentAsteroidHealth; // Máu hiện tại của thiên thạch

    [SerializeField] private TextMeshProUGUI timerText; // Thêm trường để gắn TMP

    private float timer;
    private float difficultyTimer; // Đếm thời gian để tăng độ khó

    void Start()
    {
        if (spawnPoints.Length != 5)
        {
            Debug.LogError("Hãy gán đúng 5 spawn points!");
        }

        // Khởi tạo giá trị ban đầu
        currentSpawnInterval = initialSpawnInterval;
        currentBigAsteroidSpeed = initialBigAsteroidSpeed;
        currentSmallAsteroidSpeed = initialSmallAsteroidSpeed;
        currentAsteroidHealth = 2f; // Khởi đầu: máu = 2
        difficultyTimer = 0f;
        UpdateTimerDisplay(); // Cập nhật lần đầu
    }

    void Update()
    {
        difficultyTimer += Time.deltaTime;
        UpdateDifficulty(); // Cập nhật độ khó theo thời gian
        UpdateTimerDisplay(); // Cập nhật hiển thị thời gian

        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnAsteroid();
            timer = 0f;
        }
    }

    void UpdateDifficulty()
    {
        // Cập nhật máu của thiên thạch
        if (difficultyTimer < 60f)
        {
            currentAsteroidHealth = 2f; // 0-60 giây: máu = 2
        }
        else if (difficultyTimer >= 60f && difficultyTimer < 180f)
        {
            currentAsteroidHealth = 4f; // 60-180 giây: máu = 4
        }
        else
        {
            currentAsteroidHealth = 8f; // Sau 180 giây: máu = 8
        }

        // Cập nhật tốc độ và spawn interval
        if (difficultyTimer < 15f)
        {
            // Giai đoạn 1 (0-15 giây): Giữ nguyên giá trị khởi đầu
            currentSpawnInterval = initialSpawnInterval;
            currentBigAsteroidSpeed = initialBigAsteroidSpeed;
            currentSmallAsteroidSpeed = initialSmallAsteroidSpeed;
        }
        else if (difficultyTimer >= 15f && difficultyTimer <= 120f)
        {
            // Giai đoạn 2 (15-120 giây): Tăng độ khó dần
            float t = (difficultyTimer - 15f) / (120f - 15f); // Tỷ lệ từ 0 đến 1 (từ 15s đến 120s)

            // Tăng tốc độ tuyến tính
            currentBigAsteroidSpeed = Mathf.Lerp(secondStageBigAsteroidSpeed, maxBigAsteroidSpeed, t);
            currentSmallAsteroidSpeed = Mathf.Lerp(secondStageSmallAsteroidSpeed, maxSmallAsteroidSpeed, t);

            // Giảm spawnInterval tuyến tính
            currentSpawnInterval = Mathf.Lerp(secondStageSpawnInterval, maxSpawnInterval, t);
        }
        else
        {
            // Sau 120 giây: Giữ độ khó tối đa
            currentBigAsteroidSpeed = maxBigAsteroidSpeed;
            currentSmallAsteroidSpeed = maxSmallAsteroidSpeed;
            currentSpawnInterval = maxSpawnInterval;
        }
    }

    void SpawnAsteroid()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPosition = spawnPoints[spawnIndex].position;
        Vector2 randomTarget = GetRandomPointInRectangle();

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        bool isBig = Random.value > 0.5f;
        Sprite[] spriteArray = isBig ? bigAsteroidSprites : smallAsteroidSprites;

        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = spriteArray[Random.Range(0, spriteArray.Length)];
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on Asteroid!");
        }

        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        if (asteroidScript != null)
        {
            asteroidScript.isBigAsteroid = isBig;
            asteroidScript.SetHealth(currentAsteroidHealth); // Gán máu hiện tại
        }
        else
        {
            Debug.LogError("Asteroid script not found on prefab!");
        }

        float randomRotation = Random.Range(0f, 360f);
        asteroid.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        Vector2 direction = (randomTarget - spawnPosition).normalized;

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float speed = isBig ? currentBigAsteroidSpeed : currentSmallAsteroidSpeed;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on Asteroid prefab!");
        }
    }

    Vector2 GetRandomPointInRectangle()
    {
        float halfWidth = rectangleSize.x / 2f;
        float halfHeight = rectangleSize.y / 2f;
        float randomX = Random.Range(-halfWidth, halfWidth);
        float randomY = Random.Range(-halfHeight, halfHeight);
        return (Vector2)centerPoint.position + new Vector2(randomX, randomY);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPoint.position, rectangleSize);
    }

    public void IncreaseSpeed(float increment)
    {
        currentBigAsteroidSpeed += increment;
        currentSmallAsteroidSpeed += increment;
    }

    // Cập nhật hiển thị thời gian
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(difficultyTimer / 60f);
            float seconds = Mathf.FloorToInt(difficultyTimer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}