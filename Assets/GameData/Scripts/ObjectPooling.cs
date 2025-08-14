using System.Collections;
using UnityEngine;
public class ObjectPooling : MonoBehaviour
{
    public GameObject factoryPatchPrefab;
    [SerializeField]
    GameObject[] SelectionPrefab;
    GameObject[] SelectionPool = new GameObject[20];
    public GameObject[] activeObjects = new GameObject[15];
    public Transform PlayerTransform;
    WaitForSeconds waitfor100ms = new WaitForSeconds(0.1f);
    public float SelectionLength = 30f;

    void Start()
    {
        int prefabIndex = 0;
        for (int i = 0; i < SelectionPool.Length; i++)
        {
            SelectionPool[i] = Instantiate(SelectionPrefab[prefabIndex]);
            Transform itemSpawnPoint = SelectionPool[i].transform.Find("ItemSpawner");
            RunnerGameManager.Instance.itemSpawner.SpawnOnPatch(itemSpawnPoint);
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
            randomSection.transform.position = new Vector3(SelectionPool[i].transform.position.x, 0, i * SelectionLength + SelectionLength);
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

    

    public void ReplaceThirdPatchWithFactory()
    {
        int targetIndex = -1;
        float[] distances = new float[activeObjects.Length];

        for (int i = 0; i < activeObjects.Length; i++)
        {
            distances[i] = activeObjects[i].transform.position.z - PlayerTransform.position.z;
        }
        int[] sortedIndices = new int[activeObjects.Length];
        for (int i = 0; i < sortedIndices.Length; i++) sortedIndices[i] = i;

        System.Array.Sort(sortedIndices, (a, b) => distances[a].CompareTo(distances[b]));

        int countAhead = 0;
        for (int i = 0; i < sortedIndices.Length; i++)
        {
            if (distances[sortedIndices[i]] > 0) 
            {
                countAhead++;
                if (countAhead == 3)
                {
                    targetIndex = sortedIndices[i];
                    break;
                }
            }
        }

        if (targetIndex == -1)
        {
            Debug.LogWarning("No 3rd patch found ahead of player!");
            return;
        }
        Vector3 patchPos = activeObjects[targetIndex].transform.position;
        activeObjects[targetIndex].SetActive(false);
        GameObject factoryPatch = Instantiate(factoryPatchPrefab, patchPos, Quaternion.identity, this.transform);
        activeObjects[targetIndex] = factoryPatch;
    }


}