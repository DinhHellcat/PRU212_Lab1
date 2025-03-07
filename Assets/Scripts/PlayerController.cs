﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private ProjectileController projectileLevel2;
    [SerializeField] private ProjectileController projectileLevel3;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float firingCooldown;
    [SerializeField] private float health = 4f;
    private float tempCooldown;
    private int shotLevel = 1;
    private bool isShieldActive = false;
    private bool isInvincible = false;
    private float invincibilitySpeedMultiplier = 2f;

    [SerializeField] private Sprite[] shipSprites;
    [SerializeField] private Sprite[] heartSprites;
    [SerializeField] private Image heartImage;
    private SpriteRenderer spriteRenderer;

    // Prefab hiệu ứng
    [SerializeField] private GameObject shieldEffectPrefab;
    [SerializeField] private GameObject invincibilityEffectPrefab;
    private GameObject currentEffect; // Lưu trữ hiệu ứng hiện tại

    public Vector2 minBounds = new Vector2(-5f, -5f);
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

        Vector2 moveStep = direction * Time.deltaTime * moveSpeed;
        Vector2 newPosition = (Vector2)transform.position + moveStep;

        newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

        transform.position = newPosition;

        if (Input.GetKey(KeyCode.Space))
        {
            if (tempCooldown <= 0)
            {
                Fire();
                tempCooldown = firingCooldown / (isInvincible ? invincibilitySpeedMultiplier : 1f);
            }
        }
        tempCooldown -= Time.deltaTime;
    }

    private void Fire()
    {
        ProjectileController p = GetCurrentProjectile();
        Instantiate(p, firingPoint.position, Quaternion.identity, null).handleProjectile();
    }

    private ProjectileController GetCurrentProjectile()
    {
        switch (shotLevel)
        {
            case 1: return projectile;
            case 2: return projectileLevel2;
            case 3: return projectileLevel3;
            default: return projectile;
        }
    }

    public int GetCurrentShotLevel()
    {
        return shotLevel;
    }

    public void UpgradeShot()
    {
        if (shotLevel < 3) shotLevel++;
    }

    public void ActivateShield(float duration)
    {
        isShieldActive = true;
        SpawnEffect(shieldEffectPrefab); // Kích hoạt hiệu ứng Shield
        Invoke("DeactivateShield", duration);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        DestroyEffect(); // Tắt hiệu ứng
    }

    public void ActivateInvincibility(float duration)
    {
        isInvincible = true;
        SpawnEffect(invincibilityEffectPrefab); // Kích hoạt hiệu ứng Invincibility
        Invoke("DeactivateInvincibility", duration);
    }

    private void DeactivateInvincibility()
    {
        isInvincible = false;
        DestroyEffect(); // Tắt hiệu ứng
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            if (!isShieldActive && !isInvincible)
            {
                health -= 1f;
                Debug.Log("Player health: " + health);
                UpdateSprite();
                UpdateHeartUI();
                if (health <= 0)
                {
                    Debug.Log("Player destroyed!");
                    Destroy(gameObject);
                }
            }
            else if (isShieldActive)
            {
                DeactivateShield();
                Debug.Log("Shield absorbed damage!");
            }
            Destroy(collision.gameObject);
        }
    }

    private void UpdateSprite()
    {
        int spriteIndex = Mathf.FloorToInt(health);
        if (spriteIndex >= 0 && spriteIndex < shipSprites.Length)
        {
            spriteRenderer.sprite = shipSprites[spriteIndex];
        }
    }

    private void UpdateHeartUI()
    {
        int heartIndex = Mathf.FloorToInt(health);
        if (heartIndex >= 0 && heartIndex < heartSprites.Length)
        {
            heartImage.sprite = heartSprites[heartIndex];
        }
    }

    // Kích hoạt hiệu ứng
    private void SpawnEffect(GameObject effectPrefab)
    {
        // Tắt hiệu ứng cũ (nếu có)
        DestroyEffect();

        // Tạo hiệu ứng mới làm con của Player
        currentEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);
    }

    // Tắt hiệu ứng
    private void DestroyEffect()
    {
        if (currentEffect != null)
        {
            Destroy(currentEffect);
            currentEffect = null;
        }
    }
}