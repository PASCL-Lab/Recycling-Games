
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ObjectPickupManager : MonoBehaviour
{
    public List<GameObject> pickups;
    public TextMeshProUGUI itemText;
    int NumberOfitems;
    private void Start()
    {
        pickups = new List<GameObject>();
        NumberOfitems = 0;
    }
    public void AddPickup(GameObject gameObject) 
    {
        pickups.Add(gameObject);
        gameObject.SetActive(false);
        NumberOfitems += 1;
        itemText.text = "Items: " + NumberOfitems;
    }

}
