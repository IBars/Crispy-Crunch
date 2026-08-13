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
    private bool isGameEnded = false;

    [Header("UI Elemanları")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goalText;
    public GameObject winPanel; 

    [Header("Villain Messages")]
    public TextMeshProUGUI villainMessageText; 
    private string[] villainTaunts = {
        "No, you can't win the next one!",
        "You just got lucky, don't get used to it!",
        "Did you cheat?! How did you win?!",
        "The next level will crush you!",
        "Pure coincidence... Enjoy it while it lasts.",
        "You won't escape next time!"
    };

    private void Awake()
    {
        Instance = this;
        
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        targetDestroyCount = Random.Range(10, 31); 
        maxMoves = Random.Range(3, 9); 
        currentDestroyedCount = 0;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
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

        if (currentDestroyedCount >= targetDestroyCount)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        isGameEnded = true;
        Debug.Log("LEVEL COMPLETED!");

        if (villainMessageText != null && villainTaunts.Length > 0)
        {
            int randomIndex = Random.Range(0, villainTaunts.Length);
            villainMessageText.text = villainTaunts[randomIndex];
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
            levelText.text = "Level " + currentLevel;

        if (goalText != null)
            goalText.text = "Goal: " + currentDestroyedCount + " / " + targetDestroyCount;
    }
}