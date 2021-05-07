using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Queue<EnemyPlaneController> EnemyPool;
    [SerializeField]
    private int MaxEnemies;
    [SerializeField]
    private float spawnRate;
    [SerializeField]
    private EnemyPlaneController prefab;
    [SerializeField]
    private float minRadius, maxRadius;
    [SerializeField]
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        EnemyPool = new Queue<EnemyPlaneController>(MaxEnemies);
        for(int i = 0; i < MaxEnemies; i++)
        {
            var pooledEnemy = Instantiate(prefab, transform);
            pooledEnemy.gameObject.SetActive(false);
            EnemyPool.Enqueue(pooledEnemy);
        }
        InvokeRepeating(nameof(spawnEnemy), spawnRate, spawnRate);
    }
    private void spawnEnemy()
    {
        var spawnedEnemy = EnemyPool.Dequeue();
        Vector2 position = (Vector2)(target != null ? target.position : transform.position) + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
        spawnedEnemy.transform.position = position;
        spawnedEnemy.gameObject.SetActive(true);
        spawnedEnemy.onObjectSpawn();
        EnemyPool.Enqueue(spawnedEnemy);
    }
}
