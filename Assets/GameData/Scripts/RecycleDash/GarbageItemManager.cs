using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GarbageItemManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("References to holder points on the Shelves")]
    [SerializeField] private List<PlaceHolder> listofShelfPoints1;
    [SerializeField] private List<PlaceHolder> listofShelfPoints2;
    [SerializeField] private List<PlaceHolder> listofShelfPoints3;
    [SerializeField] private List<PlaceHolder> listofShelfPoints4;
    [Header("References to the SO containing Items Prefabs")]
    [SerializeField] private TrashItemLibrary trashItemLibrary;
    
    #endregion

    
    #region Private Fields
    public List<GameObject> pickUpsList; // List of items to spawn
    private List<TrashItems> backgroundItemsList; // Items stored for later spawning
    private int itemCount = 10; // Number of items for current level
    private int itemIndex = 0; // Tracks initial spawned items
    private int nextItemIndex = 0; // Tracks next background item to move
    private const float initialYPosition = 5f;
    private const float animationDuration = 1f;
    private const float itemSpacingDelay = 0.3f;
    private const float growthRate = 1.1f;
    #endregion

    #region Public Methods

    // Main method to generate items and bins for the garbage sorting phase
    public void GenerateItems()
    {
        if (!IsSetupValid()) return;

        CalculateNumberOfItems();
        GameManager.Instance.uIManager.SetItemTextWithLevelNo(itemCount);

        pickUpsList = GenerateShuffledItemList(itemCount);
        InstantiatePickUpsToSort();

        GameManager.Instance.trashBinManager.GenerateBins();
    }
    #endregion

    #region Private Methods

    // Ensure necessary references are assigned
    private bool IsSetupValid()
    {
        if (trashItemLibrary == null)
        {
            Debug.LogError("TrashItemLibrary not assigned!");
            return false;
        }

        if (GameManager.Instance == null || GameManager.Instance.trashBinManager == null)
        {
            Debug.LogError("GameManager or TrashBinManager is not initialized!");
            return false;
        }

        return true;
    }

    // Calculate item count based on current level
    private void CalculateNumberOfItems()
    {
        int level = GameManager.Instance.GetLevel();
        itemCount += 10 * level;
        Debug.Log($"Calculated item count: {itemCount}");
    }

    // Generate a randomized and shuffled list of prefabs

    private List<GameObject> GenerateShuffledItemList(int count)
    {
        List<GameObject> items = new List<GameObject>();

        items.AddRange(trashItemLibrary.recyclablePrefabs);
        items.AddRange(trashItemLibrary.organicPrefabs);
        items.AddRange(trashItemLibrary.ewastePrefabs);
        items.AddRange(trashItemLibrary.garbagePrefabs);

        items = GenerateRandomizedList(items, count);
        GameManager.Instance.trashBinManager.ShuffleList(items);

        return items;
    }

    // Spawn all initial items onto shelf placeholders
    private void InstantiatePickUpsToSort()
    {
        backgroundItemsList = new List<TrashItems>();

        StartCoroutine(SpawnInitialItems(listofShelfPoints1));
        StartCoroutine(SpawnInitialItems(listofShelfPoints2));
        StartCoroutine(SpawnInitialItems(listofShelfPoints3));
        StartCoroutine(SpawnInitialItems(listofShelfPoints4));
        StartCoroutine(SpawnOtherItems());
    }

    // Spawn items on a single shelf with drop animation
    private IEnumerator SpawnInitialItems(List<PlaceHolder> shelfPoints)
    {
        yield return null;

        if (shelfPoints == null || pickUpsList == null)
        {
            Debug.LogError("SpawnInitialItems: ShelfPoints or PickUpsList is null!");
            yield break;
        }

        for (int i = 0; i < shelfPoints.Count && itemIndex < pickUpsList.Count; i++)
        {
            PlaceHolder point = shelfPoints[i];
            if (point == null) continue;

            GameObject itemObj = Instantiate(pickUpsList[itemIndex]);
            if (itemObj == null) continue;

            TrashItems item = itemObj.GetComponent<TrashItems>();
            if (item == null)
            {
                Debug.LogError($"Item prefab missing TrashItems component: {itemObj.name}");
                Destroy(itemObj);
                continue;
            }

            item.transform.SetParent(point.transform);
            item.transform.localPosition = new Vector3(0f, initialYPosition, 0f);
            item.transform.DOLocalMove(Vector3.zero, animationDuration)
                         .SetDelay(itemSpacingDelay * Mathf.Pow(growthRate, i + 1));
            item.transform.localScale = Vector3.one;

            itemIndex++;
        }
    }

    // Spawn remaining items off-screen to be moved later

    private IEnumerator SpawnOtherItems()
    {
        yield return new WaitForSeconds(0.5f);

        for (; itemIndex < pickUpsList.Count; itemIndex++)
        {
            GameObject itemObj = Instantiate(pickUpsList[itemIndex]);
            if (itemObj == null) continue;

            TrashItems item = itemObj.GetComponent<TrashItems>();
            if (item == null)
            {
                Debug.LogError("Missing TrashItems component on background object");
                Destroy(itemObj);
                continue;
            }

            item.transform.position = new Vector3(0f, 10f, -11.5f);
            item.transform.localScale = Vector3.one;

            backgroundItemsList.Add(item);
        }
    }

    // Move a background item onto a shelf placeholder
    public void MoveNewItem(Transform parent)
    {
        if (nextItemIndex >= backgroundItemsList.Count) return;

        TrashItems item = backgroundItemsList[nextItemIndex];
        if (item == null || parent == null) return;

        item.transform.SetParent(parent);
        item.transform.DOLocalMove(Vector3.zero, animationDuration)
                     .SetDelay(itemSpacingDelay * Mathf.Pow(growthRate, 1))
                     .OnComplete(() => item.currentPos = transform.localPosition);
        item.transform.localScale = Vector3.one;

        nextItemIndex++;
    }
    #endregion

    #region Utility
    public static List<T> GenerateRandomizedList<T>(List<T> sourceList, int targetCount)
    {
        List<T> resultList = new List<T>();

        if (sourceList == null || sourceList.Count == 0)
        {
            Debug.LogWarning("Source list is null or empty.");
            return resultList;
        }

        int sourceCount = sourceList.Count;
        int fullCopies = targetCount / sourceCount;

        for (int i = 0; i < fullCopies; i++)
        {
            resultList.AddRange(sourceList);
        }

        int remaining = targetCount - resultList.Count;
        for (int i = 0; i < remaining; i++)
        {
            resultList.Add(sourceList[Random.Range(0, sourceCount)]);
        }

        return resultList;
    }
    #endregion
}
