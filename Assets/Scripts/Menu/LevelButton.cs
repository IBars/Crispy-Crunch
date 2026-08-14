using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elemanları")]
    public TextMeshProUGUI levelText; // Prefab'ın içindeki TMP yazısı
    
    private Button button;
    private int currentLevelNumber;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnLevelClicked);
        }
    }

    // LevelNode bu fonksiyonu çağırıp numarayı verecek
    public void Setup(int levelNum)
    {
        currentLevelNumber = levelNum;

        if (levelText != null)
        {
            levelText.text = currentLevelNumber.ToString();
        }
    }

    void OnLevelClicked()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayClick();
        }

        // Tıklanan seviyeyi hafızaya kaydet
        PlayerPrefs.SetInt("CurrentLevel", currentLevelNumber);
        PlayerPrefs.Save();

        // Oyun sahnesine geç (GameScene adını kendi oyun sahnenle değiştir)
        SceneManager.LoadScene("GameScene");
    }
}