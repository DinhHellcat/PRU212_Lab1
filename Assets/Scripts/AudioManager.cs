using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgMusicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip menuBGMusic;
    [SerializeField] private AudioClip gameBGMusic;
    [SerializeField] private AudioClip asteroidDestroySFX;
    [SerializeField] private AudioClip powerUpSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip shootSFX;

    // Thêm các biến âm lượng cho từng file âm thanh (giá trị từ 0 đến 1)
    [SerializeField, Range(0f, 1f)] private float menuBGMusicVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float gameBGMusicVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float asteroidDestroySFXVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float powerUpSFXVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float buttonClickSFXVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float shootSFXVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (bgMusicSource == null)
        {
            bgMusicSource = gameObject.AddComponent<AudioSource>();
            bgMusicSource.loop = true;
            Debug.Log("Added bgMusicSource automatically.");
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("Added sfxSource automatically.");
        }

        if (menuBGMusic == null) Debug.LogWarning("Menu BG Music is not assigned!");
        if (gameBGMusic == null) Debug.LogWarning("Game BG Music is not assigned!");
        if (asteroidDestroySFX == null) Debug.LogWarning("Asteroid Destroy SFX is not assigned!");
        if (powerUpSFX == null) Debug.LogWarning("Power Up SFX is not assigned!");
        if (buttonClickSFX == null) Debug.LogWarning("Button Click SFX is not assigned!");
        if (shootSFX == null) Debug.LogWarning("Shoot SFX is not assigned!");
    }

    public void PlayMenuBGMusic()
    {
        if (bgMusicSource != null && menuBGMusic != null)
        {
            if (bgMusicSource.clip != menuBGMusic)
            {
                bgMusicSource.clip = menuBGMusic;
                bgMusicSource.volume = menuBGMusicVolume; // Áp dụng âm lượng riêng
                bgMusicSource.Play();
                Debug.Log("Playing Menu BG Music with volume: " + menuBGMusicVolume);
            }
        }
        else
        {
            Debug.LogError("Cannot play Menu BG Music: Source or clip is null.");
        }
    }

    public void PlayGameBGMusic()
    {
        if (bgMusicSource != null && gameBGMusic != null)
        {
            if (bgMusicSource.clip != gameBGMusic)
            {
                bgMusicSource.clip = gameBGMusic;
                bgMusicSource.volume = gameBGMusicVolume; // Áp dụng âm lượng riêng
                bgMusicSource.Play();
                Debug.Log("Playing Game BG Music with volume: " + gameBGMusicVolume);
            }
        }
        else
        {
            Debug.LogError("Cannot play Game BG Music: Source or clip is null.");
        }
    }

    public void StopBGMusic()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.Stop();
            Debug.Log("BG Music stopped.");
        }
    }

    public void PlayAsteroidDestroySFX()
    {
        if (sfxSource != null && asteroidDestroySFX != null)
        {
            sfxSource.PlayOneShot(asteroidDestroySFX, asteroidDestroySFXVolume); // Áp dụng âm lượng riêng
            Debug.Log("Playing Asteroid Destroy SFX with volume: " + asteroidDestroySFXVolume);
        }
        else
        {
            Debug.LogError("Cannot play Asteroid Destroy SFX: Source or clip is null.");
        }
    }

    public void PlayPowerUpSFX()
    {
        if (sfxSource != null && powerUpSFX != null)
        {
            sfxSource.PlayOneShot(powerUpSFX, powerUpSFXVolume); // Áp dụng âm lượng riêng
            Debug.Log("Playing Power Up SFX with volume: " + powerUpSFXVolume);
        }
        else
        {
            Debug.LogError("Cannot play Power Up SFX: Source or clip is null.");
        }
    }

    public void PlayButtonClickSFX()
    {
        if (sfxSource != null && buttonClickSFX != null)
        {
            sfxSource.PlayOneShot(buttonClickSFX, buttonClickSFXVolume); // Áp dụng âm lượng riêng
            Debug.Log("Playing Button Click SFX with volume: " + buttonClickSFXVolume);
        }
        else
        {
            Debug.LogError("Cannot play Button Click SFX: Source or clip is null.");
        }
    }

    public void PlayShootSFX()
    {
        if (sfxSource != null && shootSFX != null)
        {
            sfxSource.PlayOneShot(shootSFX, shootSFXVolume); // Áp dụng âm lượng riêng
            Debug.Log("Playing Shoot SFX with volume: " + shootSFXVolume);
        }
        else
        {
            Debug.LogError("Cannot play Shoot SFX: Source or clip is null.");
        }
    }
}