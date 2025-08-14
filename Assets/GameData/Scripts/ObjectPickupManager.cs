
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPickupManager : MonoBehaviour
{
    public List<int> pickups;
    public TextMeshProUGUI itemText;
    int NumberOfitems;
    private void Start()
    {
        pickups = new List<int>();
        NumberOfitems = 0;
    }
    public void AddPickup(GameObject gameObject) 
    {
        string input = gameObject.name;
        string numberString = input.Replace("Trash", "");
        int number = int.Parse(numberString);
        pickups.Add(number);
        gameObject.SetActive(false);
        NumberOfitems += 1;
        itemText.text = "Items: " + NumberOfitems;
        //if (NumberOfitems % 40 == 0)
        //{
        //    RunnerGameManager.Instance.objectPooling.ReplaceThirdPatchWithFactory();
        //}
    }

}
