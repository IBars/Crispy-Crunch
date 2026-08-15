using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene("GameScene"); 
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Sıfırlama sonrası anında görmek için Menu sahnesini tekrar yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}