using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;
    [SerializeField]
    private ParticleSystem[] prefabs;
    [SerializeField]
    private int MaxEffects;
    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }
    private void Start()
    {
        foreach(ParticleSystem prefab in prefabs)
        {
            ObjectPooler.instance.GeneratePool(prefab.name, prefab.gameObject, MaxEffects, transform);
        }
    }
    public GameObject SpawnEffectAtPosition(Vector2 position, string name)
    {
        var next = ObjectPooler.instance.GetNextInPool(name, false);
        next.transform.position = position;
        next.SetActive(true);
        return next;
    }
}
