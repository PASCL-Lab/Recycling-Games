using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    public TMP_InputField nameInputField;
    private const string PlayerNameKey = "PlayerName";
    public GameObject NamePanel;
    private const string TotalScoreKey = "TotalScore";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            NamePanel.SetActive(true);
        }
        else
        {
            NamePanel.SetActive(false);
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string savedName = PlayerPrefs.GetString("PlayerName");
            nameInputField.text = savedName;
        }
        //highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        ////Update High Score
        //if (scoreText != null)
        //    scoreText.text = "Score: " + highScore;
    }
    private const string LevelKey = "LevelNumber";
    public void StartGame()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.DeleteKey(TotalScoreKey);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GarbageSorting");
    }
    public void Continue()
    {
        SceneManager.LoadScene("GarbageSorting");
    }
    public void SavePlayerName(GameObject namePanel)
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PlayerPrefs.Save();
            Debug.Log("Player Name Saved: " + playerName);
            SetPlayerDisplayName(playerName);
            UIPopup uIPopup = namePanel.GetComponent<UIPopup>();
            uIPopup.HidePopup();
            if (!PlayerPrefs.HasKey("HasPlayedBefore"))
            {
                PlayerPrefs.SetInt("HasPlayedBefore", 1);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.LogWarning("Player name is empty");
        }
    }
    public void CloseNamePanel(GameObject namePanel)
    {

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            UIPopup uIPopup = namePanel.GetComponent<UIPopup>();
            uIPopup.HidePopup();
            Debug.Log("Name panel closed");
        }
        else
        {
            Debug.Log("Please enter a name before closing");
        }
    }
    public void SetPlayerDisplayName(string playerName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("Display name set to: " + result.DisplayName);
        },
        error =>
        {
            Debug.LogError("Failed to set display name: " + error.GenerateErrorReport());
        });
    }

    public void GetTopPlayer()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "High_Score",
            StartPosition = 0,
            MaxResultsCount = 1   // Only get the top 1
        };

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            if (result.Leaderboard.Count > 0)
            {
                var topPlayer = result.Leaderboard[0];
                Debug.Log($" Top Player: {topPlayer.DisplayName} | Score: {topPlayer.StatValue}");
                nameText.text = "Name: " + topPlayer.DisplayName;
                scoreText.text = "Scr: " + topPlayer.StatValue;

            }
            else
            {
                Debug.Log("No players found on the leaderboard yet.");
            }
        },
        error =>
        {
            Debug.LogError("Failed to fetch leaderboard: " + error.GenerateErrorReport());
        });
    }
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySound(0);
    }
    public void ToggleMuteSound()
    {
        AudioManager.Instance.ToggleSFXMute();
    }
    public void ToggleMuteMusic()
    {
        AudioManager.Instance.ToggleBGMusicMute();
    }

}
