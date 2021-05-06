using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public abstract class GunController : MonoBehaviour
{
    [SerializeField]
    private Bullet _BulletPrefab;
    [SerializeField]
    private float _Force;
    [SerializeField]
    private float _Cooldown;
    [SerializeField]
    private AudioClip _GunShot;
    [SerializeField]
    protected GunBarrel[] _Barrels;

    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        foreach (GunBarrel barrel in _Barrels)
        {
            barrel.Cooldown -= Time.deltaTime;
        }
    }

    protected void Shoot(string tag)
    {
        foreach (GunBarrel barrel in _Barrels)
        {
            if (barrel.Cooldown < 0.01f)
            {
                Bullet spawnedBullet = Instantiate(_BulletPrefab, barrel.Muzzle.position, barrel.Muzzle.rotation);
                spawnedBullet.rigidbody.velocity = rigidbody.velocity;
                spawnedBullet.rigidbody.AddForce(spawnedBullet.transform.up * _Force);
                spawnedBullet.targetTag = tag;
                barrel.Cooldown = _Cooldown;
                audioSource.PlayOneShot(_GunShot);
            }
        }
    }
}

[System.Serializable]
public class GunBarrel
{
    public Transform Muzzle;
    public float Cooldown;
}
