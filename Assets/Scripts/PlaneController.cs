using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : GridObject
{
    public new Rigidbody2D rigidbody;
    public Health health;
    protected float _Thrust = 0f;
    [SerializeField]
    protected float _MaxThrustForce;
    [SerializeField]
    private GameObject ExplosionFX;
    [SerializeField]
    private AudioClip ExplosionSound;

    // Start is called before the first frame update
    public virtual void onObjectSpawn()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.Respawn();
        addToGrid();
        health.OnDeath.AddListener(SpawnExplosion);
        health.OnDeath.AddListener(removeFromGrid);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        rigidbody.velocity = transform.up * _Thrust;
    }

    protected virtual Vector2 GetAimPos()
    {
        //TODO add quadratic solving to find aim pos.
        return transform.position + transform.up * _Thrust;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlaneController plane))
        {
            Vector2 explosionPos = (transform.position - plane.transform.position) / 2f;
            rigidbody.AddExplosionForce(10000, explosionPos, 20);
            health.Damage(5);
        }
    }
    private void SpawnExplosion()
    {
        Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(ExplosionSound, new Vector3(transform.position.x, transform.position.y, 10));
    }
    private void OnDestroy()
    {
        removeFromGrid();
    }
}
