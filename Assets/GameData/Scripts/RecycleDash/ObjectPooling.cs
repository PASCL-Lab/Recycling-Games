using System.Collections;
using UnityEngine;
public class ObjectPooling : MonoBehaviour
{
    public GameObject factoryPatchPrefab; // Prefab to replace a patch with a factory
    [SerializeField]
    GameObject[] SelectionPrefab; // Prefabs of patches to spawn
    GameObject[] SelectionPool = new GameObject[20]; // Pool of inactive patch objects
    public GameObject[] activeObjects = new GameObject[15]; // Currently active patches
    public Transform PlayerTransform;  // Player's transform to track movement
    WaitForSeconds waitfor100ms = new WaitForSeconds(0.1f);
    public float SelectionLength = 30f; // Length of each patch
    private float initialDstance;       // Starting offset for patches


    void Start()
    {
        // Determine initial distance based on whether tutorial is active
        if (RunnerGameManager.Instance.tutorial.showTutorial)
        {
            initialDstance = 5f;
        }
        else
        {
            initialDstance = 1f;
        }
        // Fill selection pool with instantiated prefabs
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
        // Activate initial patches in front of the player
        for (int i = 0; i < activeObjects.Length; i++)
        {
            GameObject randomSection = GetRandomSelection();
            randomSection.transform.position = new Vector3(SelectionPool[i].transform.position.x, 0, (i+initialDstance) * SelectionLength);
            randomSection.transform.parent = this.transform;
            randomSection.SetActive(true);
            activeObjects[i] = randomSection;
        }
        StartCoroutine(Updatelessoften());
    }

    // Coroutine to update active patches regularly
    public IEnumerator Updatelessoften()
    {
        while (true)
        {
            UpdateSelection();
            yield return waitfor100ms;
        }
    }

    // Recycle patches that have fallen behind the player
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
                Transform itemSpawnPoint = activeObjects[i].transform.Find("ItemSpawner");
                ActivateChildrens(itemSpawnPoint);
                activeObjects[i].SetActive(true);
            }
        }
    }

    // Enable all children under a given transform
    void ActivateChildrens(Transform trans)
    {
        foreach (Transform child in trans)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Pick a random inactive patch from the pool
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


    // Replace the 3rd patch ahead of player with a factory
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