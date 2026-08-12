using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;

    [Header("Ses Klipsleri")]
    public AudioClip selectSound;
    public AudioClip swapSound;
    public AudioClip explodeSound;

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

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySelect()
    {
        if (selectSound != null) audioSource.PlayOneShot(selectSound);
    }

    public void PlaySwap()
    {
        if (swapSound != null) audioSource.PlayOneShot(swapSound);
    }

    public void PlayExplode()
    {
        if (explodeSound != null) audioSource.PlayOneShot(explodeSound);
    }
}