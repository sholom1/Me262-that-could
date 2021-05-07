using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pools;

    public static ObjectPooler instance;

    private void Awake()
    {
        instance = this;
        pools = new Dictionary<string, Queue<GameObject>>();
    }
    public void GeneratePool(string name, GameObject prefab, int size, Transform parent)
    {
        if (pools.ContainsKey(name))
            throw new System.ArgumentException($"Pool with the name of {name} already exists");
        Queue<GameObject> newPool = new Queue<GameObject>(size);
        for (int i = 0; i < size; i++)
        {
            GameObject spawnedPrefab = Instantiate(prefab, parent != null ? parent : transform);
            spawnedPrefab.SetActive(false);
            newPool.Enqueue(spawnedPrefab);
        }
        pools.Add(name, newPool);
    }
    public GameObject GetNextInPool(string type, bool reuse)
    {
        if (pools.TryGetValue(type, out Queue<GameObject> value) && value.Count > 0)
        {
            GameObject next = null;
            int index = 0;
            while(next == null) { 
                next = value.Dequeue();
                if (next == null)
                    throw new System.InvalidOperationException("A pooled object has been destroyed, This is not allowed!");
                value.Enqueue(next);
                index++;
                if (reuse && next.activeInHierarchy)
                {
                    next = null;
                    if (index >= value.Count)
                        break;
                }
            }
            return next;
        }
        Debug.LogWarning($"Pool: {type} does not exist or is empty.");
        return null;
    }
    public bool ContainsPool(string name)
    {
        return pools.ContainsKey(name);
    }
}
