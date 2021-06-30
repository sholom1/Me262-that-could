using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(AudioSource))]
public class GunController : MonoBehaviour
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
    public float DamageMultiplier;

    private new Rigidbody2D rigidbody;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (!ObjectPooler.instance.ContainsPool("Bullet"))
        {
            ObjectPooler.instance.GeneratePool("Bullet", _BulletPrefab.gameObject, 100, null);
        }
    }

    protected virtual void Update()
    {
        foreach (GunBarrel barrel in _Barrels)
        {
            barrel.Cooldown -= Time.deltaTime;
        }
    }

    public void Shoot(string tag)
    {
        foreach (GunBarrel barrel in _Barrels)
        {
            if (barrel.Cooldown < 0.01f)
            {
                Bullet spawnedBullet = ObjectPooler.instance.GetNextInPool("Bullet", false).GetComponent<Bullet>();
                spawnedBullet.gameObject.SetActive(true);
                spawnedBullet.transform.position = barrel.Muzzle.position;
                spawnedBullet.transform.rotation = barrel.Muzzle.rotation;
                spawnedBullet.rigidbody.velocity = rigidbody.velocity;
                spawnedBullet.rigidbody.AddForce(spawnedBullet.transform.up * _Force);
                spawnedBullet.targetTag = tag;
                spawnedBullet.damageMultiplier = DamageMultiplier;
                barrel.Cooldown = _Cooldown;
                audioSource.PlayOneShot(_GunShot, Random.Range(.5f, 1f));
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
