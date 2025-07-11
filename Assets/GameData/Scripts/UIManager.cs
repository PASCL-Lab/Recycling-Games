using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;
    public void PauseAction(float timeScale)
    {
        if (timeScale == 0)
        {
            GameManager.Instance.gamePaused = true;
        }
        else
        {
            GameManager.Instance.gamePaused = false;
        }
        Time.timeScale = timeScale;
    }

    public void LoadSceneByIndex(int buildIndex)
    {
        if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogWarning("Invalid build index: " + buildIndex);
        }
    }
    public void ButtonClick()
    {
        AudioManager.Instance.PlaySound(0);
    }
}
