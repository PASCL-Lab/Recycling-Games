using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Lane Positions (X axis )")]
    public float lane1X = -2f;
    public float lane2X = 0f;
    public float lane3X = 2f;

    [Header("Spawn Settings")]
    public float patchLength = 20f;
    public float itemSpacing = 5f;
    public int maxObstacles = 3;

    [Header("Prefabs")]
    public GameObject[] pickupPrefab;
    public GameObject[] obstaclePrefab;

    [Header("Pickup Jump Offset")]
    public float elevatedPickupY = 2.5f;

    private float[] laneXPositions;

    void Start()
    {
        laneXPositions = new float[] { lane1X, lane2X, lane3X };
        SpawnOnPatch();
    }

    public void SpawnOnPatch()
    {
        int obstaclesSpawned = 0;
        float z = -25f;

        while (z < patchLength)
        {
            Vector3 spawnPos = new Vector3(laneXPositions[Random.Range(0,3)], 0f, transform.position.z + z);

            bool spawnObstacle = Random.value < 0.2f && obstaclesSpawned < maxObstacles;

            if (spawnObstacle)
            {
                // Spawn obstacle
                GameObject obs = Instantiate(obstaclePrefab[Random.Range(0, obstaclePrefab.Length)], spawnPos, Quaternion.identity, transform);
                obstaclesSpawned++;
                Vector3 pickupPos= Vector3.one;
                if ( obs.name.StartsWith("Hurdle")) 
                {
                    // Spawn elevated pickup above obstacle
                    pickupPos = spawnPos + Vector3.up * elevatedPickupY;
                }
                else
                {
                    pickupPos = spawnPos + Vector3.up * 0.5f;
                }
                GameObject item = Instantiate(pickupPrefab[Random.Range(0, pickupPrefab.Length)], pickupPos, Quaternion.identity, transform);
                TrashItems trash = item.GetComponent<TrashItems>();
                item.name = "Trash" + trash.itemNumber;
                item.transform.rotation = Quaternion.Euler(0, 180f, 0);
                item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(trash);

            }
            else
            {
                // Spawn regular pickup
                Vector3 pickupPos = spawnPos + Vector3.up * 1f;
                GameObject item = Instantiate(pickupPrefab[Random.Range(0,pickupPrefab.Length)], pickupPos, Quaternion.identity, transform);
                TrashItems trash = item.GetComponent<TrashItems>();
                item.name = "Trash" + trash.itemNumber;
                item.transform.rotation = Quaternion.Euler(0, 180f, 0);
                item.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                Destroy(trash);
            }

            z += itemSpacing;
        }
    }
}
