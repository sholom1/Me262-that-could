using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyPlaneController : PlaneController
{
    [SerializeField] float _StoppingDistance;
    [SerializeField] float _AttackRange;
    [SerializeField] float RotationSpeed = 20f;
    [SerializeField] GunController gunController;
    [SerializeField] int deathReward = 10;
    public override void Awake()
    {
        base.Awake();
        ScoreManager.instance.OnDifficultyIncrease.AddListener(factor =>
        {
            DifficultySettingsSO difficulty = GameManager.instance.SelectedDifficulty;
            float healthScalar = Mathf.Pow(difficulty.EnemyHealthIncrease, factor);
            health.AdjustHealthRange(healthScalar);
            gunController.DamageMultiplier = Mathf.Pow(difficulty.EnemyDamageIncrease, factor);
        });
        health.OnDeath.AddListener(() => ScoreManager.instance.AddScore(deathReward));
        OnObjectSpawn();
    }
    // Start is called before the first frame update
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        _Thrust = _MaxThrustForce;
    }
    private void Update()
    {
        if (PlayerPlaneController.instance != null)
        {
            if (Vector2.Distance(PlayerPlaneController.instance.transform.position, transform.position) < _AttackRange)
            {
                Vector2 dir = (PlayerPlaneController.instance.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(transform.up, dir);
                if (angle < 15f)
                    gunController.Shoot("Player");
            }
        }
    }
    protected override void FixedUpdate()
    {
        Vector2 dir = transform.up;
        if (PlayerPlaneController.instance != null)
        {
            Vector2 distanceVector = (PlayerPlaneController.instance.transform.position - transform.position);
            float distance = Mathf.Sqrt(distanceVector.sqrMagnitude);
            dir = distance < _StoppingDistance ? -distanceVector.normalized : distanceVector.normalized;
            //Debug.Log($"distance: {distance} stopping distance:{_StoppingDistance} bias: {DistanceBias(distance, _StoppingDistance)}");
        }
        EnemyPlaneController[] neighbors = GridManager.instance.GetGridObjectsInRadius<EnemyPlaneController>(transform.position, _StoppingDistance);
        foreach (EnemyPlaneController neighbor in neighbors)
        {
            dir += GetAvoidanceVector(neighbor, _StoppingDistance);
        }
        dir.Normalize();
        //Debug.DrawLine(transform.position, (Vector2)transform.position + dir * _StoppingDistance);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float singleStep = RotationSpeed * Time.deltaTime;
        //Debug.Log($"angle:{angle} step:{singleStep}");
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), singleStep);
        //Vector2 newDirection = Vector3.RotateTowards(transform.up, dir, singleStep, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection, Vector3.forward);
        //transform.up = Vector2.MoveTowards(transform.up, dir, Time.deltaTime);
        base.FixedUpdate();
    }
    private Vector2 GetAvoidanceVector(GridObject gridObject, float maxDistance)
    {
        Vector2 direction = (gridObject.transform.position - transform.position);
        float distance = Mathf.Sqrt(direction.sqrMagnitude);
        direction.Normalize();
        return distance < maxDistance ? -direction * DistanceBias(distance, maxDistance) : Vector2.zero;
    }
    private float DistanceBias(float distance, float maxDistance)
    {
        return (float)(-maxDistance * Math.Tanh((distance/maxDistance)-1));
    }
    private float AngleDiff(float a, float b)
    {
        float phi = Mathf.Abs(b - a) % 360;
        return phi > 180f ? 360 - phi : phi;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, PlayerPlaneController.instance.transform.position);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _StoppingDistance);
        Vector2 dir = (PlayerPlaneController.instance.transform.position - transform.position).normalized;
        Debug.Log($"angle: {Vector2.Angle(transform.up, dir)}");
    }
}
