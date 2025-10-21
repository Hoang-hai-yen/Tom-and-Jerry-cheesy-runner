using UnityEngine;
using System.Collections.Generic;

public class ItemPoolManager : MonoBehaviour
{
    public static ItemPoolManager instance;

    [System.Serializable]
    public class Pool
    {
        public string tag; 
        public GameObject prefab;
        public int initialSize; 
    }

    public List<Pool> pools; 
    
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private GameObject poolHolder;


    void Awake()
    {
        instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        poolHolder = new GameObject("ItemPoolHolder");
        poolHolder.transform.SetParent(this.transform);

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); 
                obj.transform.SetParent(poolHolder.transform);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    public GameObject GetItem(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool với tag " + tag + " không tồn tại.");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else
        {
            Debug.LogWarning("Pool " + tag + " bị hết! Đang tạo thêm.");
            Pool p = pools.Find(pool => pool.tag == tag);
            objectToSpawn = Instantiate(p.prefab);
        }

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.SetParent(null); 
        objectToSpawn.SetActive(true); 
        return objectToSpawn;
    }

    public void ReturnItem(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool với tag " + tag + " không tồn tại. Hủy đối tượng.");
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false); 
        objectToReturn.transform.SetParent(poolHolder.transform); 
        poolDictionary[tag].Enqueue(objectToReturn);
    }
}