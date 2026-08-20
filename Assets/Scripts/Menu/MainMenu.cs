using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI levelText;

    [Header("Dev Tools")]
    public int totalLevelCount = 100;

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        if (levelText != null)
            levelText.text = "LEVEL " + currentLevel;
    }

    public void PlayGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();
        SceneManager.LoadScene("GameScene");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UnlockAllLevels()
    {
        PlayerPrefs.SetInt("UnlockedLevel", totalLevelCount);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LockAllLevels()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}