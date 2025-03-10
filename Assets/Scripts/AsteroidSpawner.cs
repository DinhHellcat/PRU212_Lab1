using UnityEngine;
using TMPro;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Vector2 rectangleSize = new Vector2(4f, 2f);
    [SerializeField] private float initialSpawnInterval = 0.7f;
    [SerializeField] private float initialBigAsteroidSpeed = 3f;
    [SerializeField] private float initialSmallAsteroidSpeed = 2f;
    [SerializeField] private Sprite[] bigAsteroidSprites;
    [SerializeField] private Sprite[] smallAsteroidSprites;
    [SerializeField] private TextMeshProUGUI timerText;

    private float secondStageSpawnInterval = 0.5f;
    private float secondStageBigAsteroidSpeed = 4f;
    private float secondStageSmallAsteroidSpeed = 3f;
    private float maxSpawnInterval = 0.2f;
    private float maxBigAsteroidSpeed = 8f;
    private float maxSmallAsteroidSpeed = 6f;

    private float currentSpawnInterval;
    private float currentBigAsteroidSpeed;
    private float currentSmallAsteroidSpeed;
    private float currentAsteroidHealth;

    private float timer;
    private float difficultyTimer;

    void Start()
    {
        if (spawnPoints.Length != 5)
        {
            Debug.LogError("Hãy gán đúng 5 spawn points!");
        }

        currentSpawnInterval = initialSpawnInterval;
        currentBigAsteroidSpeed = initialBigAsteroidSpeed;
        currentSmallAsteroidSpeed = initialSmallAsteroidSpeed;
        currentAsteroidHealth = 2f;
        difficultyTimer = 0f;
        UpdateTimerDisplay();
    }

    void Update()
    {
        difficultyTimer += Time.deltaTime;
        UpdateDifficulty();
        UpdateTimerDisplay();

        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnAsteroid();
            timer = 0f;
        }
    }

    void UpdateDifficulty()
    {
        if (difficultyTimer < 60f)
        {
            currentAsteroidHealth = 2f;
        }
        else if (difficultyTimer >= 60f && difficultyTimer < 180f)
        {
            currentAsteroidHealth = 4f;
        }
        else
        {
            currentAsteroidHealth = 8f;
        }

        if (difficultyTimer < 15f)
        {
            currentSpawnInterval = initialSpawnInterval;
            currentBigAsteroidSpeed = initialBigAsteroidSpeed;
            currentSmallAsteroidSpeed = initialSmallAsteroidSpeed;
        }
        else if (difficultyTimer >= 15f && difficultyTimer <= 120f)
        {
            float t = (difficultyTimer - 15f) / (120f - 15f);
            currentBigAsteroidSpeed = Mathf.Lerp(secondStageBigAsteroidSpeed, maxBigAsteroidSpeed, t);
            currentSmallAsteroidSpeed = Mathf.Lerp(secondStageSmallAsteroidSpeed, maxSmallAsteroidSpeed, t);
            currentSpawnInterval = Mathf.Lerp(secondStageSpawnInterval, maxSpawnInterval, t);
        }
        else
        {
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
            asteroidScript.SetHealth(currentAsteroidHealth);
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

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(difficultyTimer / 60f);
            float seconds = Mathf.FloorToInt(difficultyTimer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Phương thức để lấy difficultyTimer
    public float GetDifficultyTimer()
    {
        return difficultyTimer;
    }
}