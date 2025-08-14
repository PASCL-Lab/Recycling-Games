
using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerGameManager : MonoBehaviour
{
    public static RunnerGameManager Instance { get; private set; }
    public bool playable;
    public GameObject truck;
    public GameObject factory;
    public PlayerController playerController;
    public ObjectPickupManager objectPickupManager;
    public UIManager uiManager;
    public ObjectPooling objectPooling;
    public ItemSpawner itemSpawner;
    public Image panelImage;
    public float fadeDuration = 1.5f;
    public RectTransform textTransform;

    private void Awake()
    {
        playable = false;
        Application.targetFrameRate = 60;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        ShakeText();
        //StartGame();
    }


    public void StartGame()
    {
        TruckMove();
        playerController.MoveCamera();
    }
    public void Replay()
    {
        ItemManager.itemlist = objectPickupManager.pickups.ToList();
        SceneManager.LoadScene("SortingPhase");
    }

    void TruckMove()
    {
        AudioManager.Instance.PlaySound(13);
        Sequence truckSequence = DOTween.Sequence();
        truckSequence
            .Append(truck.transform.DOMove(new Vector3(-24.5f, 0, 0), 2f))
            .Join(truck.transform.DORotate(new Vector3(0, -90, 0), 2f))
            .Append(truck.transform.DOMove(new Vector3(-4, 0, 0), 3f))
            .Append(truck.transform.DOMove(new Vector3(0, 0, 0), 3f).OnComplete(() => { playerController.StartRunning(); AudioManager.Instance.StopSoundLoop(); StartCoroutine(ThrowGarbage());}))
            .Join(truck.transform.DORotate(new Vector3(0, -180, 0), 2f))
            .Append(truck.transform.DOMove(new Vector3(0, 0, 100), 5f)).OnComplete(() =>{ truck.SetActive(false); })
            .SetEase(Ease.Linear).SetDelay(2f).OnStart(() => {
                AudioManager.Instance.PlaySoundLoop(12);
            });
    }

    

    IEnumerator ThrowGarbage()
    {
        ItemThrow(-15f,0);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(-5f,1.25f);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(5f,-1.25f);
        yield return new WaitForSeconds(0.5f);
        ItemThrow(15f, 0);
        yield return new WaitForSeconds(6f);
        factory.gameObject.SetActive(false);
        yield return null;
    }
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

    void ShakeText()
    {
        textTransform
            .DORotate(new Vector3(0, 0, 10), 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void SortItems()
    {
        ItemManager.itemlist = objectPickupManager.pickups.ToList();
        StartCoroutine(FadeOutPanel());

    }
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
        panelImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        SceneManager.LoadScene("SortingPhase");
    }
}
