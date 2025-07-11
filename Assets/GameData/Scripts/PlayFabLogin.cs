using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    public MainMenu mainMenu;
    void Awake()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab Login Successful!");
        mainMenu.GetTopPlayer();
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Error logging in: " + error.GenerateErrorReport());
    }
}
