using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Refrences")]
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI topRunText;
    private string sliderPrefKey = "ItemsValue";
    private const string HighScoreKey = "HighScore";
    [SerializeField] private TextMeshProUGUI itemNumber;
    [SerializeField] private TextMeshProUGUI levelNo;
    public TextMeshProUGUI funFactsText;
    public string[] funfacts;


    public Slider slider;
    public TextMeshProUGUI valueText;

    void Start()
    {
        if (topRunText == null) return;
        int topRun = PlayerPrefs.GetInt(HighScoreKey);
        if (topRun == 0) { topRunText.text = "00"; PlayerPrefs.SetInt(HighScoreKey, 0); }
        else { topRunText.text = topRun.ToString(); }
        float savedValue = PlayerPrefs.GetFloat(sliderPrefKey, 40f);
        slider.value = savedValue;
        UpdateValueText(slider.value);
        SaveSliderValue();
        slider.onValueChanged.AddListener(delegate { UpdateValueText(slider.value); });

    }
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

    public void SetItemText(int number)
    {
        itemNumber.text = $"Items\n{number}";
    }
    public void SetItemTextWithLevelNo(int number)
    {
        itemNumber.text = $"Items\n{number}";
        levelNo.text = $"Lvl. {GameManager.Instance.GetLevel()}";
    }
    public void Pause(float timeScale)
    {
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
    public void UnloadSceneOnTop(int buildIndex)
    {
        SceneManager.UnloadSceneAsync(buildIndex);
        Scene currentScene = SceneManager.GetActiveScene();
        foreach (GameObject obj in currentScene.GetRootGameObjects())
        {
            obj.SetActive(true);
        }
        PlayerController player = RunnerGameManager.Instance.playerController;
        player.gameObject.transform.rotation = Quaternion.identity;
        player.gameObject.transform.position = new Vector3(0, 0, player.transform.position.z);
        player.run = true;
        player.animator.SetTrigger("Run");
        player.characterSpeed = 10f;
        player.SetToNormalPos();
        RunnerGameManager.Instance.objectPickupManager.pickups = new List<int>();
        RunnerGameManager.Instance.playable = true;
        player.playerCamera = GameObject.FindGameObjectWithTag("MainGamePlayCam"); 

    }
    
    public void DisplayWinWithFunFacts()
    {
        winPanel.SetActive(true);
        if (funFactsText!=null)
        funFactsText.text = funfacts[Random.Range(0, funfacts.Length)];
    }

    void UpdateValueText(float value)
    {
        valueText.text = value.ToString("0");
    }
    public void SaveSliderValue()
    {
        float value = slider.value;
        PlayerPrefs.SetFloat(sliderPrefKey, value);
        PlayerPrefs.Save();
        RunnerGameManager.Instance.objectPickupManager.UpdateItemValue();
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
