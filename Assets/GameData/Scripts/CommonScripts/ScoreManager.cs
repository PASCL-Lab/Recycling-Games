using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText; // Current score display
    public TextMeshProUGUI highScoreText; // High score display (if used)
    public TextMeshProUGUI nameText; // Player name display


    private int currentScore = 0;
    private int highScore = 0;

    private const string HighScoreKey = "HighScore";
    private const string TotalScoreKey = "TotalScore";

    void Start()
    {
        // Display saved player name
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            nameText.text = savedName;
        }
        // Load saved high score and current score
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        currentScore = PlayerPrefs.GetInt(TotalScoreKey, 0); 
        UpdateScoreUI();
    }

    // Increment score by given amount

    public void IncrementScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        BlinkTextColor(Color.green, scoreText);
    }

    // Decrement score by given amount
    public void DecrementScore(int amount)
    {
        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;
        UpdateScoreUI();
        BlinkTextColor(Color.red, scoreText);
    }

    // Blink a text color temporarily using DOTween
    public void BlinkTextColor(Color blinkColor, TextMeshProUGUI targetText, float blinkDuration = 0.2f)
    {
        if (targetText == null) return;
        Color originalColor = targetText.color;
        DOTween.Kill(targetText);
        targetText.DOColor(blinkColor, blinkDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                targetText.DOColor(originalColor, blinkDuration).SetEase(Ease.Linear);
            });
    }

    // Check and update high score locally and on PlayFab
    public void CheckHighScore()
    {
        PlayerPrefs.SetInt(TotalScoreKey, currentScore);
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            SendScoreToLeaderboard();
        }
    }

    // Update high score specifically for the runner game
    public void RunnerGameScore()
    {
        int savedScore = PlayerPrefs.GetInt(HighScoreKey);
        int score = currentScore + GameManager.Instance.itemManager.collectedItems;
        if (score > savedScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
            SendScoreToLeaderboardRuner(score);
        }
    }
    int x = 0;

    // Update the score text UI
    private void UpdateScoreUI()
    {
        if (GameManager.Instance.itemManager != null && x==0)
        {
            Debug.Log("dfdfsdfsdf");
            currentScore = 0;
            scoreText.text = "Scr : " + currentScore ;
            x = 1;
            return;
        }
        if (scoreText != null)
            scoreText.text = "Scr : " + currentScore;
    }

    // Send score to PlayFab leaderboard for runner game
    public void SendScoreToLeaderboardRuner(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = "High_Score",
                Value = score
            }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreUpdate, OnError);
    }

    // Reset high score locally
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        highScore = 0;
        UpdateScoreUI();
    }
    // Send score to PlayFab leaderboard
    public void SendScoreToLeaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>
        {
            new StatisticUpdate
            {
                StatisticName = "High_Score",
                Value = currentScore
            }
        }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreUpdate, OnError);
    }

    // Callback for successful PlayFab update
    void OnScoreUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated score!");
    }

    // Callback for PlayFab errors
    void OnError(PlayFabError error)
    {
        Debug.LogError("Error updating score: " + error.GenerateErrorReport());
    }

}
