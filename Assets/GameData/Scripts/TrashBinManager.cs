using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrashBinManager : MonoBehaviour
{
    public GameObject recyclableBinPrefab;
    public GameObject OrganicBinPrefab;
    public GameObject garbageBinPrefab;
    public GameObject hazardousBinPrefab;
    List<TrashBins> queueOfBins;
    List<TrashItems> listOfTrashItems;
    public int maxBinCapacity = 4;
    public float distanceBetweenBins = 1.5f;

    
    public void GenerateBins()
    {
        if (GameManager.Instance == null || GameManager.Instance.pickUpManager == null)
        {
            Debug.LogError("GameManager or PickUpManager is null!");
            return;
        }
        queueOfBins = new List<TrashBins>();
        listOfTrashItems = new List<TrashItems>();
        PopulatelistofTrashItems();
        Debug.Log("Total Trash Items: " + listOfTrashItems.Count);
        List<List<TrashItems>> listOfBatches = SplitIntoBatches(listOfTrashItems, 20);
        Debug.Log("NUMBER OF BATCHES: " + listOfBatches.Count);
        foreach (List<TrashItems> list in listOfBatches)
        {
            GenerateBins(list);
        }
        PlaceTheQueue(queueOfBins);
        MoveTheQueue(queueOfBins);
        StartCoroutine(queueOfBins[0].OpenBin());
    }
    
    void PopulatelistofTrashItems()
    {
        foreach(GameObject obj in GameManager.Instance.pickUpManager.pickUpsList)
        {
            TrashItems trash = obj.GetComponent<TrashItems>();
            if (trash != null)
            {
                listOfTrashItems.Add(trash);
            }
        }
    }
    void GenerateBins(List<TrashItems> listOfItems)
    {
        int no_of_Recyclables = 0;
        int no_of_OrganicItems = 0;
        int no_of_GarbageItems = 0;
        int no_of_HazardouseItems = 0;
        foreach (TrashItems items in listOfItems)
        {
            if (items.trashType == Enums.ItemType.Recyclables) { no_of_Recyclables++; }
            else if (items.trashType == Enums.ItemType.Organic) { no_of_OrganicItems++; }
            else if (items.trashType == Enums.ItemType.Garbage) { no_of_GarbageItems++; }
            else if (items.trashType == Enums.ItemType.Hazardous_E_Waste) { no_of_HazardouseItems++; }
        }
        List<int> recycleBins = GetBinCounts(no_of_Recyclables);
        List<int> organicBins = GetBinCounts(no_of_OrganicItems);
        List<int> garbageBins = GetBinCounts(no_of_GarbageItems);
        List<int> hazardousBins = GetBinCounts(no_of_HazardouseItems);
        List<TrashBins> newBinsBatch = new List<TrashBins>();
        newBinsBatch.AddRange(InstantiateBins(recycleBins, recyclableBinPrefab, Enums.ItemType.Recyclables));
        newBinsBatch.AddRange(InstantiateBins(garbageBins, garbageBinPrefab , Enums.ItemType.Garbage));
        newBinsBatch.AddRange(InstantiateBins(organicBins, OrganicBinPrefab, Enums.ItemType.Organic));
        newBinsBatch.AddRange(InstantiateBins(hazardousBins, hazardousBinPrefab, Enums.ItemType.Hazardous_E_Waste));
        ShuffleList(newBinsBatch);
        queueOfBins.AddRange(newBinsBatch);

    }

    public void RemoveBin(TrashBins bin)
    {
        queueOfBins[0].CloseBin();
        queueOfBins.RemoveAt(0);
        MoveTheQueue(queueOfBins);
        if (queueOfBins.Count == 0)
        {
            GameManager.Instance.SortComplete(); 
        }
        else
        {
            StartCoroutine(queueOfBins[0].OpenBin());
        }
        
    }

    List<int> GetBinCounts(int itemCount)
    {
        List<int> bins = new List<int>();
        while (itemCount > 0)
        {
            int itemsInBin = Mathf.Min(maxBinCapacity, itemCount);
            bins.Add(itemsInBin);
            itemCount -= itemsInBin;
        }
        return bins;
    }
    List<TrashBins> InstantiateBins(List<int> bins, GameObject binPrefab, Enums.ItemType binType)
    {
        List<TrashBins> trashBins = new List<TrashBins>();
        foreach (int binCapacity in bins)
        {
            GameObject bin = Instantiate(binPrefab, this.transform);
            TrashBins trashBin = bin.GetComponent<TrashBins>();
            trashBin.binType = binType;
            trashBin.capacity = binCapacity;
            trashBin.text.text = binCapacity.ToString();
            trashBins.Add(trashBin);
        }
        return trashBins;
    }

    public void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    List<T> GetShuffledList<T>(List<T> originalList)
    {
        
        List<T> shuffledList = new List<T>(originalList);

        
        for (int i = 0; i < shuffledList.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, shuffledList.Count);
            T temp = shuffledList[i];
            shuffledList[i] = shuffledList[randomIndex];
            shuffledList[randomIndex] = temp;
        }

        return shuffledList;
    }


    int binIndex = 0;
    void PlaceTheQueue(List<TrashBins> list)
    {
        Debug.Log("Total Bins: " + list.Count);
        foreach (TrashBins bin in list)
        {
            bin.gameObject.transform.localPosition = new Vector3(distanceBetweenBins + (distanceBetweenBins * binIndex), 0, 0.6f);
            bin.transform.localRotation = Quaternion.Euler(0, -90, 0);
            binIndex += 1;
        }
    }
    void MoveTheQueue(List<TrashBins> list)
    {
        foreach (TrashBins bin in list)
        {
            bin.transform.DOLocalRotate(new Vector3(-12, -90, 0), 0.2f);
            bin.transform.DOLocalMoveX((bin.transform.localPosition.x - distanceBetweenBins), 0.5f).OnComplete(()=>{
                bin.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.2f);
            });
        }
    }
    public List<List<T>> SplitIntoBatches<T>(List<T> source, int batchSize)
    {
        List<List<T>> batches = new List<List<T>>();
        int totalItems = source.Count;

        for (int i = 0; i < totalItems; i += batchSize)
        {
            int currentBatchSize = Mathf.Min(batchSize, totalItems - i);
            List<T> batch = source.GetRange(i, currentBatchSize);
            batches.Add(batch);
        }

        return batches;
    }


}
