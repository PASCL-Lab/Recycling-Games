
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerGameManager : MonoBehaviour
{
    // Singleton instance
    public static RunnerGameManager Instance { get; private set; }

    // Game state flags
    public bool playable;

    [Header("References to other managers")]
    public PlayerController playerController;
    public ObjectPickupManager objectPickupManager;
    public UIManager uiManager;
    public ObjectPooling objectPooling;
    public ItemSpawner itemSpawner;
    public Tutorial tutorial;

    [Header("Fading Panel")]
    public Image panelImage; // Used for fading effect
    public float fadeDuration = 1.5f;
    [Header("Tap To Play Text")]
    public RectTransform textTransform;
    [Header("Other Refrences")]
    public GameObject truck;
    public GameObject factory;

    private void Awake()
    { 
        playable = false; 
        Application.targetFrameRate = 60; // Lock frame rate

        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        ShakeText();
        //StartGame();
    }

    // Start the game, move truck and camera
    public void StartGame()
    {
        TruckMove();
        playerController.MoveCamera();
    }

    // Replay game: pass pickups to ItemManager and load sorting scene
    public void Replay()
    {
        ItemManager.itemlist = objectPickupManager.pickups.ToList();
        SceneManager.LoadScene("SortingPhase");
    }

    // Truck movement sequence using DOTween
    void TruckMove()
    {
        AudioManager.Instance.PlaySound(13);
        Sequence truckSequence = DOTween.Sequence();
        truckSequence
            .Append(truck.transform.DOMove(new Vector3(-24.5f, 0, 0), 2f))
            .Join(truck.transform.DORotate(new Vector3(0, -90, 0), 2f))
            .Append(truck.transform.DOMove(new Vector3(-4, 0, 0), 3f))
            .Append(truck.transform.DOMove(new Vector3(0, 0, 0), 3f).OnComplete(() => { playerController.StartRunning(); AudioManager.Instance.StopSoundLoop(); StartCoroutine(ThrowGarbage()); }))
            .Join(truck.transform.DORotate(new Vector3(0, -180, 0), 2f))
            .Append(truck.transform.DOMove(new Vector3(0, 0, 100), 5f)).OnComplete(() => { truck.SetActive(false); })
            .SetEase(Ease.Linear).SetDelay(2f).OnStart(() =>
            {
                AudioManager.Instance.PlaySoundLoop(12);
            });
    }


    // Coroutine to throw trash items at specific positions
    IEnumerator ThrowGarbage()
    {
        ItemThrow(-15f, 0);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(-5f, 1.25f);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(5f, -1.25f);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(15f, 0);
        //yield return new WaitForSeconds(6f);
        //factory.gameObject.SetActive(false);
        yield return null;
    }

    // Spawn and animate a single trash item
    void ItemThrow(float pos, float pos1)
    {
        Vector3 pickupPos = new Vector3(pos1, 0, pos) + Vector3.up * 1f;
        GameObject item = Instantiate(itemSpawner.pickupPrefab[Random.Range(0, itemSpawner.pickupPrefab.Length)], truck.transform.position, Quaternion.identity);
        item.transform.rotation = Quaternion.Euler(0, 180f, 0);
        item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        item.transform.DOJump(pickupPos, 2f, 1, 1);
        TrashItems trash = item.GetComponent<TrashItems>();
        item.name = "Trash" + trash.itemNumber;
        Destroy(trash);
    }

    // Animate UI text shake for visual effect
    void ShakeText()
    {
        textTransform
            .DORotate(new Vector3(0, 0, 10), 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    // Transfer collected items to ItemManager and fade panel before sorting
    public void SortItems()
    {
        ItemManager.itemlist = new List<int>();
        ItemManager.itemlist = objectPickupManager.pickups.ToList();
        StartCoroutine(FadeOutPanel());

    }

    // Fade panel in and switch to sorting scene
    private IEnumerator FadeOutPanel()
    {
        if (panelImage == null) yield break;

        Color startColor = panelImage.color;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 1f, elapsed / fadeDuration);
            panelImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        SceneManager.LoadScene("SortingPhase",LoadSceneMode.Additive);
        Scene currentScene = SceneManager.GetActiveScene();
        foreach (GameObject obj in currentScene.GetRootGameObjects())
        {
            if (!obj.CompareTag("NeverInActive"))
            {
                obj.SetActive(false);
            }
            
        }
        playerController.run = false;
        panelImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    // Coroutine triggered when player hits an obstacle and loses
    public IEnumerator ObstacleHit()
    {
        playable = false;
        yield return new WaitForSeconds(1);
        uiManager.losePanel.SetActive(true);
    }
}
