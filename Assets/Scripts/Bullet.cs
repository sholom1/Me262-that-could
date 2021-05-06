using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public string targetTag;
    [SerializeField]
    private float minBulletLiftime = 1;
    [SerializeField]
    private int damage;
    [SerializeField]
    private GameObject DebrisParticles;

    private void FixedUpdate()
    {
        if (minBulletLiftime < 0.01f && rigidbody.velocity.sqrMagnitude < 100)
            Destroy(gameObject);
        minBulletLiftime -= Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag) && collision.TryGetComponent(out Health health))
        {
            Debug.Log($"Damaged {health.gameObject.name}");
            health.Damage(damage);
            Instantiate(DebrisParticles, transform.position, Quaternion.identity);
            Destroy(gameObject, .1f);
        }
    }
}
