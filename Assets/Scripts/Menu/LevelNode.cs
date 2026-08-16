using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [Header("Level Ayarı")]
    public int levelNumber;

    [Header("Prefab")]
    public GameObject unlockedButtonPrefab;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelNumber <= unlockedLevel)
        {
            Image lockedImage = GetComponent<Image>();
            if (lockedImage != null)
            {
                lockedImage.enabled = false;
            }

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            GameObject newButtonObj = Instantiate(unlockedButtonPrefab, transform);

            // Instantiate(prefab, parent) world pozisyonunu koruduğu için
            // yeni objeyi parent'ın merkezine sabitliyoruz:
            RectTransform rt = newButtonObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
                rt.localRotation = Quaternion.identity;
            }

            LevelButton levelBtn = newButtonObj.GetComponent<LevelButton>();
            if (levelBtn != null)
            {
                levelBtn.Setup(levelNumber);

                // Oyuncunun sırada oynayacağı (en son açılan) level ise parıltı efektini başlat
                if (levelNumber == unlockedLevel)
                {
                    levelBtn.PlayHighlight();
                }
            }
        }
    }
}