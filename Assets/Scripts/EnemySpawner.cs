using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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
        ObjectPooler.instance.GeneratePool(nameof(EnemyPlaneController), prefab.gameObject, MaxEnemies, transform);
        InvokeRepeating(nameof(spawnEnemy), spawnRate, spawnRate);
    }
    private void spawnEnemy()
    {
        var spawnedEnemyGO = ObjectPooler.instance.GetNextInPool(nameof(EnemyPlaneController), true);
        if (spawnedEnemyGO != null)
        {
            var spawnedEnemy = spawnedEnemyGO.GetComponent<EnemyPlaneController>();
            Vector2 position = (Vector2)(target != null ? target.position : transform.position) + Random.insideUnitCircle * Random.Range(minRadius, maxRadius);
            spawnedEnemy.transform.position = position;
            spawnedEnemy.gameObject.SetActive(true);
            spawnedEnemy.OnObjectSpawn();
        }
    }
}
