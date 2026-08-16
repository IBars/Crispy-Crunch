using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Dev Tool Ayarı")]
    public int totalLevelCount = 32; // toplam level sayına göre güncelle

    public void PlayGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
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