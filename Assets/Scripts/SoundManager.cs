using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip clickSound;
    public AudioClip explodeSound;
    public AudioClip swipeSound;
    public AudioClip winSound;       // Kazanma sesi
    public AudioClip gameOverSound;  // Kötü adam kahkahası (Game Over)

    private void Awake()
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
    }

    public void PlayClick()
    {
        if (clickSound != null && sfxSource != null)
            sfxSource.PlayOneShot(clickSound);
    }

    public void PlayExplode()
    {
        if (explodeSound != null && sfxSource != null)
            sfxSource.PlayOneShot(explodeSound);
    }

    public void PlaySwap() // Tile.cs'deki hata burada çözülüyor
    {
        if (swipeSound != null && sfxSource != null)
            sfxSource.PlayOneShot(swipeSound);
    }

    public void PlaySelect() // Tile seçildiğinde çalması için
    {
        if (clickSound != null && sfxSource != null)
            sfxSource.PlayOneShot(clickSound);
    }

    public void PlayWin()
    {
        if (winSound != null && sfxSource != null)
            sfxSource.PlayOneShot(winSound);
    }

    public void PlayGameOver()
    {
        if (gameOverSound != null && sfxSource != null)
            sfxSource.PlayOneShot(gameOverSound);
    }
}