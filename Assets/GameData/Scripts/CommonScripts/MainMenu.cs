using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyUI.Toast;
public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;      // Top player score display
    public TextMeshProUGUI nameText;       // Top player name display
    public TMP_InputField nameInputField;  // Input field for player name
    public GameObject NamePanel;           // Panel to enter player name
    public TextMeshProUGUI[] nameTexts;    // Leaderboard text fields (top 3)

    private const string PlayerNameKey = "PlayerName";
    private const string TotalScoreKey = "TotalScore";
    private const string LevelKey = "LevelNumber";

    // Show name panel if first time playing
    private void Awake()
    {
        NamePanel.SetActive(!PlayerPrefs.HasKey("HasPlayedBefore"));
    }

    private void Start()
    {
        // Load saved player name if exists
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            nameInputField.text = PlayerPrefs.GetString(PlayerNameKey);
        }
    }

    // Start a new game: reset level and score
    public void StartGame()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.DeleteKey(TotalScoreKey);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GarbageSorting");
    }

    // Continue game without resetting data
    public void Continue()
    {
        SceneManager.LoadScene("GarbageSorting");
    }

    // Save entered player name and hide name panel
    public void SavePlayerName(GameObject namePanel)
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PlayerPrefs.Save();
            SetPlayerDisplayName(playerName,namePanel);
            if (!PlayerPrefs.HasKey("HasPlayedBefore"))
            {
                PlayerPrefs.SetInt("HasPlayedBefore", 1);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Toast.Show("Please enter a name", 1.5f, ToastColor.Magenta);
        }
    }

    // Close name panel only if player has entered a name
    public void CloseNamePanel(GameObject namePanel)
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            namePanel.GetComponent<UIPopup>().HidePopup();
            Debug.Log("Name panel closed");
        }
        else
        {
            Toast.Show("Please enter a name before closing", 1.5f, ToastColor.Magenta);
        }
    }

    // Update PlayFab display name for the player
    public void SetPlayerDisplayName(string playerName, GameObject namePanel)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = playerName };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("Display name set to: " + result.DisplayName);
            UIPopup uIPopup = namePanel.GetComponent<UIPopup>();
            uIPopup.HidePopup();
        },
        error =>
        {
            Toast.Show("Name not Available",1.5f, ToastColor.Magenta);
        });
    }

    // Fetch top player from PlayFab leaderboard
    public void GetTopPlayer()
    {
        var request = new GetLeaderboardRequest { StatisticName = "High_Score", StartPosition = 0, MaxResultsCount = 1 };

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            if (result.Leaderboard.Count > 0)
            {
                var topPlayer = result.Leaderboard[0];
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
            Toast.Show("Failed to Fetch LeaderBoard", 1.5f, ToastColor.Magenta);
        });
    }

    // Fetch top 3 players and display in UI
    public void GetTopThreePlayers()
    {
        var request = new GetLeaderboardRequest { StatisticName = "High_Score", StartPosition = 0, MaxResultsCount = 3 };

        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var player = result.Leaderboard[i];
                nameTexts[i].text = $"{player.DisplayName} : {player.StatValue}";
            }
        },
        error =>
        {
            Toast.Show("Failed to Fetch LeaderBoard", 1.5f, ToastColor.Magenta);
        });
    }

    // Button sound effect
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySound(0);
    }

    // Toggle SFX mute
    public void ToggleMuteSound()
    {
        AudioManager.Instance.ToggleSFXMute();
    }

    // Toggle background music mute
    public void ToggleMuteMusic()
    {
        AudioManager.Instance.ToggleBGMusicMute();
    }
}
