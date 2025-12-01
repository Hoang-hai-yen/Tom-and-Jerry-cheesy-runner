using UnityEngine;
using System.Collections.Generic;

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
    public float despawnDistanceBehind = 40f;

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    private Transform poolHolder;

    private List<GameObject> activeChunks = new List<GameObject>();
    private Transform nextConnectionPoint;
    private float lastPlayerZ = -Mathf.Infinity;

    void Awake()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        InitializePoolHolder();

        GameObject initialChunk = SpawnNewChunk(startChunk, Vector3.zero, Quaternion.identity);
        nextConnectionPoint = initialChunk.transform.Find("ConnectionPoint");

        lastPlayerZ = playerTransform.position.z;

        for (int i = 1; i < initialChunks; i++)
            SpawnChunk();
    }

    void Update()
    {
        if (playerTransform.position.z > lastPlayerZ)
        {
            lastPlayerZ = playerTransform.position.z;

            if (playerTransform.position.z > nextConnectionPoint.position.z - spawnDistanceAhead)
                SpawnChunk();

            CleanupOldChunks();
        }
    }


    void SpawnChunk()
    {
        GameObject prefab = mapChunkPrefabs[Random.Range(0, mapChunkPrefabs.Length)];

        GameObject chunk = GetChunk(prefab);

        chunk.transform.position = nextConnectionPoint.position;
        chunk.transform.rotation = nextConnectionPoint.rotation;

        activeChunks.Add(chunk);

        nextConnectionPoint = chunk.transform.Find("ConnectionPoint");
        if (nextConnectionPoint == null)
            Debug.LogError("Prefab thiếu ConnectionPoint: " + chunk.name);
    }

    GameObject SpawnNewChunk(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject chunk = GetChunk(prefab);

        chunk.transform.position = pos;
        chunk.transform.rotation = rot;

        activeChunks.Add(chunk);

        return chunk;
    }


    GameObject GetChunk(GameObject prefab)
    {
        string key = prefab.name;

        if (!pool.ContainsKey(key))
            pool[key] = new Queue<GameObject>();

        if (pool[key].Count > 0)
        {
            GameObject chunk = pool[key].Dequeue();
            chunk.SetActive(true);
            chunk.transform.SetParent(null); 
            ResetChunk(chunk);
            return chunk;
        }

        GameObject newChunk = Instantiate(prefab);
        newChunk.name = key;
        ResetChunk(newChunk);
        return newChunk;
    }

    void ReturnChunk(GameObject chunk)
    {
        string key = chunk.name;

        chunk.SetActive(false);
        chunk.transform.SetParent(poolHolder); 
        pool[key].Enqueue(chunk);
    }

    void ResetChunk(GameObject chunk)
    {
        chunk.transform.localScale = Vector3.one;
    }

    void CleanupOldChunks()
    {
        List<GameObject> toReturn = new List<GameObject>();

        foreach (var chunk in activeChunks)
        {
            if (chunk.transform.position.z < playerTransform.position.z - despawnDistanceBehind)
                toReturn.Add(chunk);
        }

        foreach (var c in toReturn)
        {
            activeChunks.Remove(c);
            ReturnChunk(c);
        }
    }

    void InitializePoolHolder()
    {
        GameObject obj = new GameObject("RoadSegmentPool");
        obj.transform.SetParent(transform);
        poolHolder = obj.transform;
    }
}
