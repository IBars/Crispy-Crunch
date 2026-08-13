using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // 1. Level'de olduğumuzu PlayerPrefs ile hafızaya kaydediyoruz (ileride seviye yazısını güncellemek için işimize yarayacak)
        PlayerPrefs.SetInt("CurrentLevel", 1);
        
        // Oyun sahnesini yüklüyoruz. ("GameScene" yazan yeri kendi oyun sahnenin adıyla değiştir)
        SceneManager.LoadScene("GameScene"); 
    }
}