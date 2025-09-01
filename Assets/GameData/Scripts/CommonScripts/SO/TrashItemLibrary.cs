using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TrashItemLibrary", menuName = "Game/TrashItemLibrary")]
public class TrashItemLibrary : ScriptableObject
{
    //Scriptable object to store the Prefabs of Items
    public List<GameObject> recyclablePrefabs;
    public List<GameObject> organicPrefabs;
    public List<GameObject> garbagePrefabs;
    public List<GameObject> ewastePrefabs;

}
