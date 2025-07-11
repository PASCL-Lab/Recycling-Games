using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TrashItemLibrary", menuName = "Game/TrashItemLibrary")]
public class TrashItemLibrary : ScriptableObject
{
    public List<GameObject> recyclablePrefabs;
    public List<GameObject> organicPrefabs;
    public List<GameObject> garbagePrefabs;
    public List<GameObject> ewastePrefabs;

}
