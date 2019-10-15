using System.Collections.Generic;
using UnityEngine;


public class AssetPool
{
    private Transform poolsRoot;

    private Dictionary<string, GameObject> poolPrefabs = 
        new Dictionary<string, GameObject>();
    private Dictionary<string, Transform> poolTransfroms = 
        new Dictionary<string, Transform>();
    private Dictionary<string, Queue<GameObject>> poolContents = 
        new Dictionary<string, Queue<GameObject>>();
        
    public void CreatePool(string poolName, GameObject prefab)
    {
        poolPrefabs[poolName] = prefab;

        var poolTransform = new GameObject(poolName).transform;
        if (!poolsRoot)
        {
            poolsRoot = new GameObject("pools").transform;
        }
        poolTransform.SetParent(poolsRoot);
        poolTransfroms[poolName] = poolTransform;

        poolContents[poolName] = new Queue<GameObject>();
    }

    public GameObject TakeFrom(string poolName)
    {
        if (!poolPrefabs.ContainsKey(poolName))
        {
            return null;
        }
        
        var pool = poolContents[poolName];
        if (pool.Count < 1)
        {
            var prefab = poolPrefabs[poolName];
            var newGo = GameObject.Instantiate(prefab, poolTransfroms[poolName]);
            newGo.SetActive(false);
            pool.Enqueue(newGo);
        }
        return pool.Dequeue();
    }

    public void PutTo(string poolName, GameObject go)
    {
        go.SetActive(false);
        poolContents[poolName].Enqueue(go);
    }
}
