
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPickupManager : MonoBehaviour
{
    int itemValue; // Number of items needed to trigger factory
    public List<int> pickups; // Collected item numbers
    public TextMeshProUGUI itemText; // UI text showing collected items
    public int NumberOfitems; // Counter for collected items
    private void Start()
    {
        pickups = new List<int>();
        NumberOfitems = 0;
        itemValue = (int) PlayerPrefs.GetFloat("ItemsValue");
    }

    // Add collected item to the list and update UI
    public void AddPickup(GameObject gameObject) 
    {
        string input = gameObject.name;
        string numberString = input.Replace("Trash", "");
        int number = int.Parse(numberString);
        pickups.Add(number);
        gameObject.SetActive(false);
        NumberOfitems += 1;
        itemText.text = "Items: " + NumberOfitems;
        if (NumberOfitems % itemValue == 0)
        {
            RunnerGameManager.Instance.objectPooling.ReplaceThirdPatchWithFactory();
        }
    }

    // Update the threshold value from PlayerPrefs
    public void UpdateItemValue()
    {
        itemValue = (int)PlayerPrefs.GetFloat("ItemsValue");
    }

}
