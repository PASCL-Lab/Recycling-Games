using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Lane Positions (X axis )")]
    [SerializeField] private float lane1X = -2f;
    [SerializeField] private float lane2X = 0f;
    [SerializeField] private float lane3X = 2f;

    [Header("Spawn Settings")]
    [SerializeField] private float patchLength = 20f;  // Length of a single patch to spawn items
    [SerializeField] private float itemSpacing = 5f;   // Distance between consecutive items
    [SerializeField] private int maxObstacles = 3;     // Max obstacles per patch


    [Header("Prefabs")]
    public GameObject[] pickupPrefab; // Array of pickup prefabs
    public GameObject[] obstaclePrefab;  // Array of obstacle prefabs

    [Header("Pickup Jump Offset")]
    [SerializeField] private float elevatedPickupY = 2.5f; // Height for pickups above obstacles

    private float[] laneXPositions;  // Cached lane positions for random selection



    void Start()
    {
        laneXPositions = new float[] { lane1X, lane2X, lane3X };
    }


    // Spawn pickups and obstacles on a patch
    public void SpawnOnPatch(Transform targetPatch)
    {
        //Debug.Log(laneXPositions.Length);
        //if(laneXPositions.Length != 3)
        //{
        //    laneXPositions = new float[] { lane1X, lane2X, lane3X };
        //}
        int obstaclesSpawned = 0;
        float z = -25f;
        while (z < patchLength)
        {
            // Random lane position for this item
            Vector3 spawnPos = new Vector3(laneXPositions[Random.Range(0, 3)], 0f, targetPatch.position.z + z);

            bool spawnObstacle = Random.value < 0.2f && obstaclesSpawned < maxObstacles;

            if (spawnObstacle)
            {
                // Spawn obstacle
                GameObject obs = Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Length)], spawnPos, Quaternion.identity, targetPatch);
                obstaclesSpawned++;
                Vector3 pickupPos = Vector3.one;
                if (obs.CompareTag("Hurdle"))
                {
                    // Spawn elevated pickup above obstacle
                    pickupPos = spawnPos + Vector3.up * elevatedPickupY;
                }
                else
                {
                    // Regulaer Height
                    pickupPos = spawnPos + Vector3.up * 0.5f;
                }
                SpawnPickup(pickupPos, targetPatch);

            }
            else
            {
                // Regulaer Height
                Vector3 pickupPos = spawnPos + Vector3.up * 1f;
                SpawnPickup(pickupPos, targetPatch);
            }

            z += itemSpacing;
        }
    }

    // Spawn pickup at the Given Positionand under the given parent
    private void SpawnPickup(Vector3 pos, Transform parent)
    {
        GameObject item = Instantiate(pickupPrefab[Random.Range(0, pickupPrefab.Length)], pos, Quaternion.identity, parent);
        TrashItems trash = item.GetComponent<TrashItems>();
        item.name = "Trash" + trash.itemNumber;
        item.transform.rotation = Quaternion.Euler(0, 180f, 0);
        item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        Destroy(trash);
    }
}
