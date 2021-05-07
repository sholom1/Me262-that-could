using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyPlaneController : PlaneController
{
    [SerializeField] float _StoppingDistance;
    [SerializeField] float RotationSpeed = 20f;
    private void Start()
    {
        onObjectSpawn();
    }
    // Start is called before the first frame update
    public override void onObjectSpawn()
    {
        base.onObjectSpawn();
        _Thrust = _MaxThrustForce;
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
        Debug.DrawLine(transform.position, (Vector2)transform.position + dir * _StoppingDistance);
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
}
