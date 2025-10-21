using UnityEngine;
using System.Collections.Generic;

// CẤP 1: QUẢN LÝ KHÚC ĐƯỜNG
public class MapSpawner : MonoBehaviour
{
    [Header("Map Prefabs")]
    public GameObject startChunk;
    public GameObject[] mapChunkPrefabs; 

    [Header("Player Settings")]
    public Transform playerTransform;

    [Header("Spawning Settings")]
    public int initialChunks = 1;
    public float spawnDistanceAhead = 100f;
    public float despawnDistanceBehind = 30f;

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    private GameObject poolHolder;

    private List<GameObject> activeChunks = new List<GameObject>();
    private Transform nextConnectionPoint; 
    private float lastPlayerZ = -Mathf.Infinity;

    void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        InitializePoolHolder();

        GameObject initialChunk = GetChunkFromPool(startChunk);
        initialChunk.transform.position = Vector3.zero;
        initialChunk.transform.rotation = Quaternion.identity;
        activeChunks.Add(initialChunk);
        nextConnectionPoint = initialChunk.transform.Find("ConnectionPoint");

        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
        lastPlayerZ = playerTransform.position.z;
    }

    void Update()
    {
        if (playerTransform.position.z > lastPlayerZ)
        {
            lastPlayerZ = playerTransform.position.z;

            if (nextConnectionPoint != null &&
                playerTransform.position.z > (nextConnectionPoint.position.z - spawnDistanceAhead))
            {
                SpawnChunk();
            }

            CleanupOldChunks();
        }
    }

    private void SpawnChunk()
    {
        GameObject randomPrefab = mapChunkPrefabs[Random.Range(0, mapChunkPrefabs.Length)];

        GameObject newChunk = GetChunkFromPool(randomPrefab);
        
        newChunk.transform.position = nextConnectionPoint.position;
        newChunk.transform.rotation = nextConnectionPoint.rotation;

        activeChunks.Add(newChunk);

        nextConnectionPoint = newChunk.transform.Find("ConnectionPoint");

        if (nextConnectionPoint == null)
        {
            Debug.LogError("LỖI: Prefab " + newChunk.name + " không có 'ConnectionPoint'!");
        }
    }

    private void CleanupOldChunks()
    {
        List<GameObject> chunksToReturn = new List<GameObject>();
        foreach (GameObject chunk in activeChunks)
        {
            Transform connection = chunk.transform.Find("ConnectionPoint");
            if (connection == null) continue;

            if (connection.position.z < (playerTransform.position.z - despawnDistanceBehind))
            {
                chunksToReturn.Add(chunk);
            }
        }

        foreach (GameObject chunk in chunksToReturn)
        {
            activeChunks.Remove(chunk);
            ReturnChunkToPool(chunk); 
        }
    }


    private GameObject GetChunkFromPool(GameObject prefab)
    {
        string poolKey = prefab.name;
        
        if (pool.ContainsKey(poolKey) && pool[poolKey].Count > 0)
        {
            GameObject chunk = pool[poolKey].Dequeue();
            chunk.SetActive(true);
            return chunk;
        }
        else
        {
            GameObject newChunk = Instantiate(prefab);
            newChunk.name = poolKey; 
            newChunk.transform.SetParent(poolHolder.transform); 
            newChunk.SetActive(false); 
            ReturnChunkToPool(newChunk);
            
            GameObject chunk = pool[poolKey].Dequeue();
            chunk.SetActive(true);
            return chunk;
        }
    }

    private void ReturnChunkToPool(GameObject chunk)
    {
        string poolKey = chunk.name;
        chunk.SetActive(false);
        chunk.transform.SetParent(poolHolder.transform);

        if (!pool.ContainsKey(poolKey))
        {
            pool[poolKey] = new Queue<GameObject>();
        }
        pool[poolKey].Enqueue(chunk);
    }

    private void InitializePoolHolder()
    {
        poolHolder = new GameObject("RoadSegmentPool");
        poolHolder.transform.SetParent(this.transform);
    }
}