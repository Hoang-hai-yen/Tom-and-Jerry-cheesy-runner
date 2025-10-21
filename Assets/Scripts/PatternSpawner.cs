using UnityEngine;
using System.Collections.Generic;

public class PatternSpawner : MonoBehaviour
{
    private List<GameObject> activeItems = new List<GameObject>();

    void OnEnable()
    {
        SpawnItemsFromChildren();
    }

    void OnDisable()
    {
        ReturnAllItemsToPool();
    }

    void SpawnItemsFromChildren()
    {
        activeItems.Clear(); 

        foreach (Transform childPoint in transform)
        {
            if (!childPoint.gameObject.activeSelf) continue;

            string[] nameParts = childPoint.name.Split('_');
            if (nameParts.Length < 2 || nameParts[0] != "SpawnPoint")
            {
                Debug.LogWarning("Tên con không hợp lệ: " + childPoint.name, childPoint);
                continue; 
            }

            string itemTag = nameParts[1]; 

            GameObject spawnedItem = ItemPoolManager.instance.GetItem(
                itemTag,
                childPoint.position,
                childPoint.rotation
            );

            if (spawnedItem != null)
            {
                ItemIdentifier identifier = spawnedItem.GetComponent<ItemIdentifier>();
                if(identifier == null) 
                    identifier = spawnedItem.AddComponent<ItemIdentifier>();
                
                identifier.itemTag = itemTag;
                
                activeItems.Add(spawnedItem);
            }
        }
    }

    void ReturnAllItemsToPool()
    {
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                if (!item.activeSelf)
                {
                    continue; 
                }

                ItemIdentifier identifier = item.GetComponent<ItemIdentifier>();
                if (identifier != null && identifier.itemTag != null)
                {
                    ItemPoolManager.instance.ReturnItem(identifier.itemTag, item);
                }
                else
                {
                    item.SetActive(false); 
                }
            }
        }
        activeItems.Clear();
    }
}