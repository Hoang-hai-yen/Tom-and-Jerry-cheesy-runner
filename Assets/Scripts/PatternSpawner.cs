using UnityEngine;
using System.Collections.Generic;

public class PatternSpawner : MonoBehaviour
{
    [Header("Lane Settings")]
    public int laneCount = 3;                       
    public float laneWidth = 2f;                   

    [Header("Spawn Points")]
    public List<SpawnPointConfig> spawnPoints;     

    [Header("Despawn Settings")]
    public float despawnZ = -20f;                  

    private List<GameObject> activeItems = new List<GameObject>();

    void OnEnable()
    {
        SpawnAllItems();
    }
    public void PrepareForDespawn()
    {
        DespawnAllItems();
    }
    private void SpawnAllItems()
    {
        if (spawnPoints == null || spawnPoints.Count == 0) return;

        activeItems.Clear();

        foreach (var config in spawnPoints)
        {
            if (config == null || string.IsNullOrEmpty(config.itemTag)) continue;
            if (Random.Range(0f, 100f) > config.spawnChance) continue;

            Vector3 spawnPos = config.transform.position; 
            GameObject item = ItemPoolManager.instance.GetItem(config.itemTag);
            if (item != null)
            {
                item.transform.position = spawnPos;
                item.transform.rotation = config.transform.rotation;
                item.transform.SetParent(this.transform);
                item.SetActive(true);
                activeItems.Add(item);
            }
        }
    }


    private void DespawnAllItems()
    {
        foreach (var item in activeItems)
        {
            if (item == null) continue;

            ItemTagHolder tagHolder = item.GetComponent<ItemTagHolder>();

            if (tagHolder != null)
            {
                item.transform.SetParent(null); 
                ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, item);
            }
            else
            {
                item.SetActive(false);
                item.transform.SetParent(null);
            }
        }

        activeItems.Clear();
    }

    public void CleanupBehindPlayer(float playerZ)
    {
        if (activeItems == null) return;

        List<GameObject> toRemove = new List<GameObject>();

        foreach (var item in activeItems)
        {
            if (item == null) continue;

            if (item.transform.position.z < playerZ + despawnZ)
            {
                toRemove.Add(item);
            }
        }

        foreach (var item in toRemove)
        {
            activeItems.Remove(item);
            ItemTagHolder tagHolder = item.GetComponent<ItemTagHolder>();
            if (tagHolder != null && !string.IsNullOrEmpty(tagHolder.itemTag))
            {
                ItemPoolManager.instance.ReturnItem(tagHolder.itemTag, item);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
}
