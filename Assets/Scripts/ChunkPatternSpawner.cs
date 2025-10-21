using UnityEngine;
using System.Collections.Generic;

public class ChunkPatternSpawner : MonoBehaviour
{
    [System.Serializable]
    public class AnchorPatternLink
    {
        public string description; 
        public Transform anchorPoint;
        
        public GameObject[] spawnablePatterns; 
        
        [Range(0, 100)]
        public float spawnChance = 100f;

        [System.NonSerialized] 
        public int lastSpawnedIndex = -1;
    }

    [Header("Pattern Links")]
    public List<AnchorPatternLink> patternLinks;

    private List<GameObject> patternPool = new List<GameObject>();

    void OnEnable()
    {
        SpawnPatterns();
    }

    void OnDisable()
    {
        DeactivateAllPatterns();
    }

    void SpawnPatterns()
    {
        if (patternLinks == null || patternLinks.Count == 0) return;

        DeactivateAllPatterns();

        foreach (AnchorPatternLink link in patternLinks)
        {
            if (link.anchorPoint == null || link.spawnablePatterns.Length == 0)
            {
                continue;
            }

            if (Random.Range(0f, 100f) > link.spawnChance)
            {
                continue;
            }

            int prefabIndex;
            if (link.spawnablePatterns.Length > 1)
            {
                do {
                    prefabIndex = Random.Range(0, link.spawnablePatterns.Length);
                } while (prefabIndex == link.lastSpawnedIndex);
            }
            else
            {
                prefabIndex = 0; 
            }
            
            link.lastSpawnedIndex = prefabIndex;
            GameObject patternPrefab = link.spawnablePatterns[prefabIndex];

            GameObject patternInstance = FindInPool(patternPrefab.name);

            if (patternInstance != null)
            {
                patternInstance.transform.position = link.anchorPoint.position;
                patternInstance.transform.rotation = link.anchorPoint.rotation;
                patternInstance.SetActive(true);
            }
            else
            {
                patternInstance = Instantiate(
                    patternPrefab, 
                    link.anchorPoint.position, 
                    link.anchorPoint.rotation
                );
                patternInstance.name = patternPrefab.name; 
                patternInstance.transform.SetParent(this.transform);
                patternPool.Add(patternInstance); 
            }
        }
    }

    void DeactivateAllPatterns()
    {
        foreach (GameObject pattern in patternPool)
        {
            if (pattern != null)
            {
                pattern.SetActive(false); 
            }
        }
        foreach (AnchorPatternLink link in patternLinks)
        {
            link.lastSpawnedIndex = -1;
        }
    }

    GameObject FindInPool(string prefabName)
    {
        foreach (GameObject pattern in patternPool)
        {
            if (pattern.name == prefabName && !pattern.activeSelf)
            {
                return pattern;
            }
        }
        return null;
    }
}