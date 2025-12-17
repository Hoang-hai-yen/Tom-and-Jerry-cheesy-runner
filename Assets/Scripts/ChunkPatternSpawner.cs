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

    public void PrepareForDespawn()
    {
        ReturnActivePatternsToPool();
    }

    void SpawnPatterns()
    {
        bool isFlying = PlayerFlyController.Instance != null && PlayerFlyController.Instance.IsFlying;

        foreach (var link in patternLinks)
        {
            if (link.anchorPoint == null) continue;
            string finalTag = isFlying ? GetAirTag(link.poolTag) : link.poolTag;

            if (Random.Range(0f, 100f) > link.spawnChance) continue;

            GameObject patternObj = ItemPoolManager.instance.GetItem(finalTag);
            if (patternObj == null) continue;

            patternObj.transform.position = link.anchorPoint.position;
            patternObj.transform.rotation = link.anchorPoint.rotation;
            patternObj.transform.SetParent(this.transform);
            patternObj.SetActive(true);
            link.activePattern = patternObj;
        }
    }

    string GetAirTag(string groundTag) => groundTag + "_Air";

    public void ReturnActivePatternsToPool()
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
