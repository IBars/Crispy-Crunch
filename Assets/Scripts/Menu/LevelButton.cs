using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elemanları")]
    public TextMeshProUGUI levelText;

    [Header("Parıltı Efekti (opsiyonel)")]
    public Image glowImage;
    public GameObject sparklePrefab;
    public RectTransform sparkleSpawnArea;

    private Button button;
    private int currentLevelNumber;
    private Coroutine highlightRoutine;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnLevelClicked);
    }

    public void Setup(int levelNum)
    {
        currentLevelNumber = levelNum;
        if (levelText != null)
            levelText.text = currentLevelNumber.ToString();
    }

    public void PlayHighlight()
    {
        if (highlightRoutine == null)
            highlightRoutine = StartCoroutine(HighlightLoop());
    }

    private IEnumerator HighlightLoop()
    {
        Vector3 baseScale = transform.localScale;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * 1.5f, 1f);
            float scaleOffset = Mathf.Lerp(0f, 0.08f, t);
            transform.localScale = baseScale * (1f + scaleOffset);

            if (glowImage != null)
            {
                Color c = glowImage.color;
                c.a = Mathf.Lerp(0.2f, 0.6f, t);
                glowImage.color = c;
            }

            if (sparklePrefab != null && Random.value < 0.02f)
                SpawnSparkle();

            yield return null;
        }
    }

    private void SpawnSparkle()
    {
        RectTransform area = sparkleSpawnArea != null ? sparkleSpawnArea : (RectTransform)transform;
        GameObject sparkle = Instantiate(sparklePrefab, area);
        RectTransform sparkleRT = sparkle.GetComponent<RectTransform>();

        float radius = area.rect.width * 0.4f;
        sparkleRT.anchoredPosition = Random.insideUnitCircle * radius;

        StartCoroutine(AnimateSparkle(sparkleRT));
    }

    private IEnumerator AnimateSparkle(RectTransform sparkleRT)
    {
        Image img = sparkleRT.GetComponent<Image>();
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            float scale = Mathf.Sin(progress * Mathf.PI);
            sparkleRT.localScale = Vector3.one * scale;

            if (img != null)
            {
                Color c = img.color;
                c.a = scale;
                img.color = c;
            }
            yield return null;
        }

        Destroy(sparkleRT.gameObject);
    }

    void OnLevelClicked()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();

        PlayerPrefs.SetInt("CurrentLevel", currentLevelNumber);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}