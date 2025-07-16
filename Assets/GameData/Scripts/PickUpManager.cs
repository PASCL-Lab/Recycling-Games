using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    int itemCount;
    public List<PlaceHolder> listofShelfPoints1;
    public List<PlaceHolder> listofShelfPoints2;
    public List<PlaceHolder> listofShelfPoints3;
    public List<PlaceHolder> listofShelfPoints4;
    public TrashItemLibrary trashItemLibrary;
    public List<GameObject> pickUpsList;
    [SerializeField] TextMeshProUGUI itemNumber;
    [SerializeField] TextMeshProUGUI levelNo;
    public int itemIndex = 0;
    int nextItemIndex = 0;

    private void Awake()
    {
        itemCount = 10;
        CalculateNumberOfItems();
        SetUI();
        pickUpsList = new List<GameObject>();
        pickUpsList.AddRange(trashItemLibrary.recyclablePrefabs);
        pickUpsList.AddRange(trashItemLibrary.organicPrefabs);
        pickUpsList.AddRange(trashItemLibrary.ewastePrefabs);
        pickUpsList.AddRange(trashItemLibrary.garbagePrefabs);
        pickUpsList = GenerateRandomizedList(pickUpsList, itemCount);
        GameManager.Instance.trashBinManager.ShuffleList(pickUpsList);
        InstantiatePickUpsToSort();
        GameManager.Instance.trashBinManager.GenerateBins();
    }
    void SetUI()
    {
        itemNumber.text = "Items\n" + itemCount;
        levelNo.text = "Lvl. " + GameManager.Instance.GetLevel();
    }
    void CalculateNumberOfItems()
    {
        int levelNumber = GameManager.Instance.GetLevel();
        for (int i = 0; i < levelNumber; i++) {
            itemCount += 10;
        }
        Debug.Log(itemCount);
    }
    public void InstantiatePickUpsToSort()
    {
        if (listofShelfPoints1 == null)
        {
            Debug.LogError("listofShelfPoints1 is null!");
        }
        if (pickUpsList == null)
        {
            Debug.LogError("pickup is null!");
        }
        Debug.Log("Total pickUpsList count: " + pickUpsList.Count);
        StartCoroutine(SpawnInitialItems(listofShelfPoints1, pickUpsList));
        StartCoroutine(SpawnInitialItems(listofShelfPoints2, pickUpsList));
        StartCoroutine(SpawnInitialItems(listofShelfPoints3, pickUpsList));
        StartCoroutine(SpawnInitialItems(listofShelfPoints4, pickUpsList));
        backgroundItemsList = new List<TrashItems>();
        StartCoroutine(SpawnOtherItems(pickUpsList));
    }
    public IEnumerator SpawnInitialItems(List<PlaceHolder> listOfShelfPoints, List<GameObject> itemsList)
    {
        yield return null;
        float startValue = 0.3f;
        float growthRate = 1.1f;

        for (int i = 1; i <= listOfShelfPoints.Count; i++)
        {
            if (itemIndex >= itemsList.Count) break;
            GameObject itemObj = Instantiate(itemsList[itemIndex]);
            TrashItems item = itemObj.GetComponent<TrashItems>();
            item.transform.SetParent(listOfShelfPoints[i - 1].transform);
            item.transform.localPosition = new Vector3(0f, 5f, 0f);
            item.transform.DOLocalMove(Vector3.zero, 1f).SetDelay(startValue * Mathf.Pow(growthRate, i)).OnComplete(() => { item.currentPos = transform.localPosition; });
            //item.transform.localPosition = Vector3.zero;
            //item.currentPos = transform.localPosition;
            item.transform.localScale = Vector3.one;
            itemIndex++;
        }
        //nextItemIndex = itemIndex;
    }
    public IEnumerator SpawnOtherItems(List<GameObject> itemsList)
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("ItemIndex" + itemIndex);
        for (; itemIndex < itemsList.Count; itemIndex++)
        {
            GameObject itemObj = Instantiate(itemsList[itemIndex]);
            TrashItems item = itemObj.GetComponent<TrashItems>();
            item.transform.position = new Vector3(0f, 10f, -11.5f);
            //item.transform.DOLocalMove(Vector3.zero, 1f).SetDelay(startValue * Mathf.Pow(growthRate, i)).OnComplete(() => { item.currentPos = transform.localPosition; });
            item.transform.localScale = Vector3.one;
            backgroundItemsList.Add(item);
        }
    }
    List<TrashItems> backgroundItemsList;
    public void MoveNewItem(Transform parent)
    {
        float startValue = 0.3f;
        float growthRate = 1.1f;
        if (nextItemIndex < backgroundItemsList.Count)
        {
            TrashItems item = backgroundItemsList[nextItemIndex];
            item.transform.parent = parent;
            item.transform.DOLocalMove(Vector3.zero, 1f).SetDelay(startValue * Mathf.Pow(growthRate, 1)).OnComplete(() => { item.currentPos = transform.localPosition; });
            item.transform.localScale = Vector3.one;
            nextItemIndex++;
        }

    }
    public static List<T> GenerateRandomizedList<T>(List<T> sourceList, int targetCount)
    {
        List<T> resultList = new List<T>();

        int sourceCount = sourceList.Count;

        if (sourceCount == 0)
        {
            Debug.LogWarning("Source list is empty.");
            return resultList;
        }

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
}
