using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [Header("References to other managers")]
    // References to other managers
    public GarbageItemManager garbageItemManager;
    public InputManager inputManager;
    public ItemManager itemManager;
    public TrashBinManager trashBinManager;
    public ScoreManager scoreManager;
    public UIManager uIManager;
    public Camera mainCamera;
    public Timer timer;

    [HideInInspector] public bool gamePaused;

    private const string LevelKey = "LevelNumber"; // Key for saving level in PlayerPrefs

    private void Awake()
    {
        gamePaused = false;
        // Implement singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Generate initial items depending on which manager is available
        if (garbageItemManager != null) {
            garbageItemManager.GenerateItems();
        }
        else
        {
            trashBinManager.phase = false;
            itemManager.GenerateItems();
        }
        
    }

    // Called when sorting is completed successfully
    public void SortComplete()
    {
        IncrementLevel();
        scoreManager.CheckHighScore();
        StartCoroutine(DelayedExecution());
    }

    // Called to continue to next run after sorting
    public void SortToRun()
    {
        StartCoroutine(DelayedExecution());
        scoreManager.RunnerGameScore();
    }

    // Handles delayed actions like showing win panel and fun fact
    IEnumerator DelayedExecution()
    {
        yield return new WaitForSeconds(1.2f);
        uIManager.DisplayWinWithFunFacts();
        AudioManager.Instance.PlaySound(8);
        uIManager.PauseAction(0);
    }


    // Called when sorting fails
    public void SortFailed()
    {
        uIManager.losePanel.SetActive(true);
        AudioManager.Instance.PlaySound(7);
        uIManager.PauseAction(0);
    }

    // Get current level from PlayerPrefs
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LevelKey,1);
    }

    // Increment level and save
    public void IncrementLevel()
    {
        int currentLevel = GetLevel();
        currentLevel++;
        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    // Reset level to default
    public void ResetLevel()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.Save();
    }


}
