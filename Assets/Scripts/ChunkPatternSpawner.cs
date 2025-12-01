using UnityEngine;
using System.Collections.Generic;

public class ChunkPatternSpawner : MonoBehaviour
{
    [System.Serializable]
    public class AnchorPatternLink
    {
        public string description;
        public Transform anchorPoint;
        public string poolTag;              
        [Range(0, 100)]
        public float spawnChance = 100f;
        [System.NonSerialized]
        public GameObject activePattern;   
    }

    [Header("Pattern Links")]
    public List<AnchorPatternLink> patternLinks;

    void OnEnable()
    {
        SpawnPatterns();
    }

    void OnDisable()
    {
        ReturnActivePatternsToPool();
    }

    void SpawnPatterns()
    {
        if (patternLinks == null || patternLinks.Count == 0) return;

        foreach (var link in patternLinks)
        {
            if (link.anchorPoint == null || string.IsNullOrEmpty(link.poolTag)) continue;
            if (Random.Range(0f, 100f) > link.spawnChance) continue;

            // Lấy pattern từ pool
            GameObject patternPrefab = ItemPoolManager.instance.GetItem(link.poolTag);
            if (patternPrefab == null) continue;

            patternPrefab.transform.position = link.anchorPoint.position;
            patternPrefab.transform.rotation = link.anchorPoint.rotation;
            patternPrefab.transform.SetParent(this.transform);
            patternPrefab.SetActive(true);

            link.activePattern = patternPrefab;
        }
    }

    void ReturnActivePatternsToPool()
    {
        if (patternLinks == null) return;

        foreach (var link in patternLinks)
        {
            if (link.activePattern != null)
            {
                ItemPoolManager.instance.ReturnItem(link.poolTag, link.activePattern);
                link.activePattern = null;
            }
        }
    }
}
