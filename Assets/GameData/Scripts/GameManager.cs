using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PickUpManager pickUpManager;
    public InputManager inputManager;
    public TrashBinManager trashBinManager;
    public ScoreManager scoreManager;
    public UIManager uIManager;
    public Camera mainCamera;
    public Timer timer;
    [HideInInspector] public bool gamePaused;

    private const string LevelKey = "LevelNumber";

    private void Awake()
    {
        gamePaused = false;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void SortComplete()
    {
        IncrementLevel();
        scoreManager.CheckHighScore();
        StartCoroutine(DelayedExecution());
    }

    IEnumerator DelayedExecution()
    {
        yield return new WaitForSeconds(1.2f);
        uIManager.winPanel.SetActive(true);
        AudioManager.Instance.PlaySound(8);
        uIManager.PauseAction(0);
    }

    public void SortFailed()
    {
        uIManager.losePanel.SetActive(true);
        AudioManager.Instance.PlaySound(7);
        uIManager.PauseAction(0);
    }

    
    public int GetLevel()
    {
        return PlayerPrefs.GetInt(LevelKey,1);
    }

    public void IncrementLevel()
    {
        int currentLevel = GetLevel();
        currentLevel++;
        PlayerPrefs.SetInt(LevelKey, currentLevel);
        PlayerPrefs.Save();
    }

    public void ResetLevel()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.Save();
    }


}
