using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
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
    public  List<TrashItems> pickUpsList; // List of items to be spawned
    public static  List<int> itemlist; // Static list of item numbers for current run
    private List<TrashItems> backgroundItemsList; // Items waiting to be spawned later
    private int itemIndex = 0; // Tracks initial items spawned
    private int nextItemIndex = 0; // Tracks next item from background list
    private const float initialYPosition = 5f;
    private const float animationDuration = 1f;
    private const float itemSpacingDelay = 0.3f;
    private const float growthRate = 1.1f;
    public int collectedItems; // Total items collected
    #endregion

    #region Public Methods

    // Entry point to generate all items and bins for the level
    public void GenerateItems()
    {
         
        if (!IsSetupValid()) return;
        collectedItems = itemlist.Count;
        GameManager.Instance.uIManager.SetItemText(collectedItems); // Update UI text

        pickUpsList = GenerateList(itemlist); // Generate list of TrashItems from numbers
        InstantiatePickUpsToSort(); // Spawn items on shelves

        GameManager.Instance.trashBinManager.GenerateBins();  // Spawn bins
    }
    #endregion

    #region Private Methods
    // Ensure necessary references exist
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

    // Start spawning items on all shelf lists and background
    private void InstantiatePickUpsToSort()
    {
        backgroundItemsList = new List<TrashItems>();

        StartCoroutine(SpawnInitialItems(listofShelfPoints1));
        StartCoroutine(SpawnInitialItems(listofShelfPoints2));
        StartCoroutine(SpawnInitialItems(listofShelfPoints3));
        StartCoroutine(SpawnInitialItems(listofShelfPoints4));
        StartCoroutine(SpawnOtherItems());
    }

    // Spawn initial items on a single shelf with animation
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

            GameObject itemObj = Instantiate(pickUpsList[itemIndex].gameObject);
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

    // Spawn remaining items in background for later use

    private IEnumerator SpawnOtherItems()
    {
        yield return new WaitForSeconds(0.5f);

        for (; itemIndex < pickUpsList.Count; itemIndex++)
        {
            GameObject itemObj = Instantiate(pickUpsList[itemIndex].gameObject);
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

    // Move a background item onto a shelf
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

    List<TrashItems> GenerateList(List<int> itemNumbers)
    {
        List<TrashItems> pickedItems = new List<TrashItems>();
        foreach (int i in itemNumbers) {
            foreach(TrashItems trash in pickUpsList)
            {
                if (trash.itemNumber == i) { pickedItems.Add(trash); }
            }
        }
        return pickedItems;
    }

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

    #endregion
}
