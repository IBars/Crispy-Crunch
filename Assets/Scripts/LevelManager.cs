using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Ayarları")]
    public int currentLevel = 1;
    public int targetDestroyCount;
    public int maxMoves;
    public int currentDestroyedCount = 0;
    public bool isGameEnded = false;

    [Header("UI Elemanları")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goalText;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Villain Messages")]
    public TextMeshProUGUI villainMessageText;
    public TextMeshProUGUI gameOverMessageText;

    private string[] winTaunts = {
        "No, you can't win the next one!",
        "You just got lucky!",
        "How did you win?!",
        "The next level will crush you!"
    };

    private string[] lossTaunts = {
        "I told you you couldn't defeat me!",
        "Ha! Not even close, human!",
        "Did you really think you could win?",
        "Better luck next time... you'll need it!",
        "I always win in the end!",
        "You gave it a try, but I am unstoppable!"
    };

    private void Awake()
    {
        Instance = this;

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        targetDestroyCount = Random.Range(10, 31);
        maxMoves = Random.Range(3, 9);
        currentDestroyedCount = 0;

        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Start()
    {
        UpdateLevelUI();
    }

    public void AddDestroyedBlock(int amount = 1)
    {
        if (isGameEnded) return;
        currentDestroyedCount += amount;
        UpdateLevelUI();
    }

    public bool HasReachedGoal()
    {
        return currentDestroyedCount >= targetDestroyCount;
    }

    public void CheckWinAfterExplosions()
    {
        if (isGameEnded) return;

        if (HasReachedGoal())
        {
            TriggerWin();
        }
        else
        {
            TriggerGameOver();
        }
    }

    public void TriggerWin()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWin();
        }

        if (villainMessageText != null && winTaunts.Length > 0)
            villainMessageText.text = winTaunts[Random.Range(0, winTaunts.Length)];

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int nextLevel = currentLevel + 1;
        if (nextLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();
        }

        if (winPanel != null) winPanel.SetActive(true);
    }

    public void TriggerGameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;
        Debug.Log("GAME OVER!");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayGameOver();
        }

        if (gameOverMessageText != null && lossTaunts.Length > 0)
            gameOverMessageText.text = lossTaunts[Random.Range(0, lossTaunts.Length)];

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void GoToMap()
    {
        if (SoundManager.Instance != null) SoundManager.Instance.PlayClick();
        SceneManager.LoadScene("Menu");
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Level " + currentLevel;

        if (goalText != null)
        {
            goalText.text = "Goal: " + currentDestroyedCount + " / " + targetDestroyCount;

            float progress = (float)currentDestroyedCount / targetDestroyCount;
            progress = Mathf.Clamp01(progress);

            
        }
    }
}