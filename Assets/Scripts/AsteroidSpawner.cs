using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private Vector2 rectangleSize = new Vector2(4f, 2f);
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Sprite[] bigAsteroidSprites;
    [SerializeField] private Sprite[] smallAsteroidSprites;
    [SerializeField] private float bigAsteroidSpeed = 2f;
    [SerializeField] private float smallAsteroidSpeed = 3f;

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
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Vector2 spawnPosition = spawnPoints[spawnIndex].position;
        Vector2 randomTarget = GetRandomPointInRectangle();

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        bool isBig = Random.value > 0.5f;
        Sprite[] spriteArray = isBig ? bigAsteroidSprites : smallAsteroidSprites;

        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteArray[Random.Range(0, spriteArray.Length)];

        // Gán giá trị isBigAsteroid
        Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
        asteroidScript.isBigAsteroid = isBig;

        float randomRotation = Random.Range(0f, 360f);
        asteroid.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        Vector2 direction = (randomTarget - spawnPosition).normalized;

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        float speed = isBig ? bigAsteroidSpeed : smallAsteroidSpeed;
        rb.linearVelocity = direction * speed;
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
        bigAsteroidSpeed += increment;
        smallAsteroidSpeed += increment;
    }
}