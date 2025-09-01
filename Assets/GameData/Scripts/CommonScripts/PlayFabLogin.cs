using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    // Reference to MainMenu script to update leaderboard after login
    public MainMenu mainMenu;
    void Awake()
    {
        // Prepare login request using device's unique ID
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier, // Unique ID per device
            CreateAccount = true // Automatically create an account if none exists
        };
        // Perform login with PlayFab
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    // Called when login succeeds and Set the LeaderBoard

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab Login Successful!");
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Menu")
        {
            Debug.Log("You are in the Menu scene.");
            mainMenu.GetTopPlayer();
        }
        else if (currentScene == "MainGameplay")
        {
            Debug.Log("You are in the Gameplay scene.");
            mainMenu.GetTopThreePlayers();
        }
        
    }

    // Called when login fails
    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Error logging in: " + error.GenerateErrorReport());
    }
}
