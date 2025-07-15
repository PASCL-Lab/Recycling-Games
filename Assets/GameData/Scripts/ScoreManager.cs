using DG.Tweening;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI nameText;
    private const string PlayerNameKey = "PlayerName";

    private int currentScore = 0;
    private int highScore = 0;

    private const string HighScoreKey = "HighScore";

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            nameText.text = savedName;
        }
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UpdateScoreUI();
    }

    public void IncrementScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
        BlinkTextColor(Color.green, scoreText);
    }

    public void DecrementScore(int amount)
    {
        currentScore -= amount;
        if (currentScore < 0) currentScore = 0;
        UpdateScoreUI();
        BlinkTextColor(Color.red, scoreText);
    }
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

    public void CheckHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
            SendScoreToLeaderboard();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Scr : " + currentScore;
    }


    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        highScore = 0;
        UpdateScoreUI();
    }
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

    void OnScoreUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated score!");
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Error updating score: " + error.GenerateErrorReport());
    }

}
