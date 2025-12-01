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
        public int size;
    }

    public List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> poolDict;
    private Transform poolHolder;

    void Awake()
    {
        instance = this;

        poolDict = new Dictionary<string, Queue<GameObject>>();
        poolHolder = new GameObject("ITEM_POOL").transform;
        poolHolder.SetParent(transform);

        foreach (var p in pools)
        {
            Queue<GameObject> q = new Queue<GameObject>();

            for (int i = 0; i < p.size; i++)
            {
                GameObject obj = Instantiate(p.prefab, poolHolder);
                obj.SetActive(false);
                q.Enqueue(obj);
            }

            poolDict.Add(p.tag, q);
        }
    }

    public GameObject Get(string tag)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' không tồn tại!");
            return null;
        }

        if (poolDict[tag].Count == 0)
        {
            Pool p = pools.Find(x => x.tag == tag);
            if (p != null)
            {
                GameObject newObj = Instantiate(p.prefab);
                newObj.SetActive(true);
                newObj.transform.SetParent(null);
                return newObj;
            }

            Debug.LogWarning($"Pool '{tag}' trống và không tìm thấy prefab để tạo thêm.");
            return null;
        }

        GameObject obj = poolDict[tag].Dequeue();

        obj.transform.SetParent(null);      
        obj.SetActive(true);

        return obj;
    }

    public void Return(string tag, GameObject obj)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool '{tag}' không tồn tại — Destroy object.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);

        obj.transform.SetParent(poolHolder);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        poolDict[tag].Enqueue(obj);
    }

    public GameObject GetItem(string tag)
    {
        return Get(tag);
    }

    public void ReturnItem(string tag, GameObject obj)
    {
        Return(tag, obj);
    }
}
