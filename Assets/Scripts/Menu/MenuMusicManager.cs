using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager Instance;

    [Header("Müzik Ayarları")]
    public AudioSource musicSource;
    public AudioClip menuMusic;

    private void Awake()
    {
        // Singleton yapısı: Sahne değişse de müziğin kesilmemesini sağlar
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (musicSource != null && menuMusic != null)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.clip = menuMusic;
                musicSource.loop = true; // Şarkı bitince başa sarar
                musicSource.Play();
            }
        }
    }

    public void StopMenuMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}