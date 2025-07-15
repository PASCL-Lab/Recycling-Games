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


}
