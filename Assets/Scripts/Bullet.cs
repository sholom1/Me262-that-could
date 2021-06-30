using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public string targetTag;
    [SerializeField]
    private float minBulletLiftime = 1;
    private float lifeTime;
    [SerializeField]
    private int damage;
    [SerializeField]
    private GameObject DebrisParticles;

    public float damageMultiplier;

    private void OnEnable()
    {
        lifeTime = 0;
    }
    private void FixedUpdate()
    {
        if (lifeTime >= minBulletLiftime && rigidbody.velocity.sqrMagnitude < 100)
            gameObject.SetActive(false);
        lifeTime += Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && collision.TryGetComponent(out Health health))
        {
            Debug.Log($"Damaged {health.gameObject.name}");
            health.Damage(Mathf.CeilToInt(damage * damageMultiplier));
            EffectsManager.instance.SpawnEffectAtPosition(transform.position, "HitImpact");
            gameObject.SetActive(false);
        }
    }
}
