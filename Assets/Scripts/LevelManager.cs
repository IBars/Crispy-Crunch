using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Ayarları")]
    public int currentLevel = 1;
    public int targetDestroyCount; // Rastgele belirlenecek hedef
    public int maxMoves;           // Rastgele belirlenecek hamle sayısı
    public int currentDestroyedCount = 0;

    [Header("UI Elemanları")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goalText;

    private void Awake()
    {
        Instance = this;
        
        // Level bilgisini al
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // RASTGELE HEDEF VE HAMLE BELİRLEME
        // Hedef: Minimum 10, Maksimum 30 blok (İstediğin aralığı verebilirsin)
        targetDestroyCount = Random.Range(10, 31); 

        // Hamle: Minimum 3, Maksimum 8 hamle
        maxMoves = Random.Range(3, 9); 

        currentDestroyedCount = 0;
    }

    void Start()
    {
        UpdateLevelUI();
    }

    public void AddDestroyedBlock(int amount = 1)
    {
        currentDestroyedCount += amount;
        UpdateLevelUI();

        if (currentDestroyedCount >= targetDestroyCount)
        {
            Debug.Log("LEVEL TAMAMLANDI!");
            // Seviye bitiş/kazanma mantığı için hazırdız
        }
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Level " + currentLevel;

        if (goalText != null)
            goalText.text = "Goal: " + currentDestroyedCount + " / " + targetDestroyCount;
    }
}