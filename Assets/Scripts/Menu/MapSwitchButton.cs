using UnityEngine;
using UnityEngine.UI;

public class MapSwitchButton : MonoBehaviour
{
    [Header("Harita Geçişi")]
    public GameObject currentMap;
    public GameObject targetMap;

    [Header("Kilit Kontrolü")]
    public bool requiresUnlock = false;   // İleri ok için true, geri ok için false
    public int requiredLevelToUnlock;      // Bu haritadaki son level numarası

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchMap);
    }

    void OnEnable()
    {
        if (requiresUnlock)
        {
            int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
            // Bu haritanın son leveli açılmadıysa oku gizle
            gameObject.SetActive(unlockedLevel > requiredLevelToUnlock);
        }
    }

    void SwitchMap()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();

        currentMap.SetActive(false);
        targetMap.SetActive(true);
    }
}