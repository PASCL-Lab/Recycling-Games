using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI References")]
    public Slider loadingSlider;  // Slider showing loading progress

    [Header("Settings")]
    public float loadDuration = 3f;  // Duration to fill the slider
    public string firstTimeScene = "TutorialScene"; // Scene for first-time players
    public string nextTimeScene = "MainMenu";  // Scene for returning players

    private float timer; // Tracks elapsed time

    void Start()
    {
        if (loadingSlider != null)
            loadingSlider.value = 0f; // Reset slider at start

        timer = 0f;
    }

    void Update()
    {
        if (loadingSlider == null) return;

        timer += Time.deltaTime;
        loadingSlider.value = Mathf.Clamp01(timer / loadDuration);

        if (loadingSlider.value >= 1f)
        {
            // Check if this is the first time the player runs the game
            if (PlayerPrefs.GetInt("FirstTimePlayed", 0) == 0)
            {
                PlayerPrefs.SetInt("FirstTimePlayed", 1); 
                PlayerPrefs.Save();
                SceneManager.LoadScene(firstTimeScene);
            }
            else
            {
                SceneManager.LoadScene(nextTimeScene);
            }
        }
    }
}
