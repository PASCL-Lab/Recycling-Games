using System.Collections;
using UnityEngine;
public class ObjectPooling : MonoBehaviour
{
    [SerializeField]
    GameObject[] SelectionPrefab;
    GameObject[] SelectionPool = new GameObject[20];
    GameObject[] activeObjects = new GameObject[15];
    public Transform PlayerTransform;
    WaitForSeconds waitfor100ms = new WaitForSeconds(0.1f);
    public float SelectionLength = 30f;
    
    void Start()
    {
        int prefabIndex = 0;
        for (int i = 0; i < SelectionPool.Length; i++)
        {
            SelectionPool[i] = Instantiate(SelectionPrefab[prefabIndex]);
            SelectionPool[i].SetActive(false);
            SelectionPool[i].transform.parent = this.transform;
            prefabIndex++;
            if (prefabIndex > SelectionPrefab.Length - 1)
            {
                prefabIndex = 0;
            }
        }
        for (int i = 0; i < activeObjects.Length; i++)
        {
            GameObject randomSection = GetRandomSelection();
            randomSection.transform.position = new Vector3(SelectionPool[i].transform.position.x, 0, i * SelectionLength);
            randomSection.transform.parent = this.transform;
            randomSection.SetActive(true);
            activeObjects[i] = randomSection;
        }
        StartCoroutine(Updatelessoften());
    }
    IEnumerator Updatelessoften()
    {
        while (true)
        {
            UpdateSelection();
            yield return waitfor100ms;
        }
    }
    void UpdateSelection()
    {
        for (int i = 0; i < activeObjects.Length; i++)
        {
            if (activeObjects[i].transform.position.z - PlayerTransform.transform.position.z < -SelectionLength)
            {
                //Debug.Log("Patch Position: "+Selection[i].transform.position.z+" Player Position : "+PlayerTransform.transform.position.z);
                Vector3 LastPosition = activeObjects[i].transform.position;
                activeObjects[i].SetActive(false);
                activeObjects[i] = GetRandomSelection();
                activeObjects[i].transform.position = new Vector3(LastPosition.x, 0, LastPosition.z + SelectionLength * activeObjects.Length);
                activeObjects[i].transform.parent = this.transform;
                activeObjects[i].SetActive(true);
            }
        }
    }
    GameObject GetRandomSelection()
    {
        int randomIndex = Random.Range(0, SelectionPool.Length);
        bool isnewsection = false;
        while (!isnewsection)
        {
            if (!SelectionPool[randomIndex].activeInHierarchy)
            {
                isnewsection = true;
            }
            else
            {
                randomIndex++;
                if (randomIndex > SelectionPool.Length - 1)
                {
                    randomIndex = 0;
                }
            }
        }
        return SelectionPool[randomIndex];
    }
   
}