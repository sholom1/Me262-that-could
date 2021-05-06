using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneController : PlaneController
{
    [SerializeField] float _StoppingDistance;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _Thrust = _MaxThrustForce;
    }
    protected override void FixedUpdate()
    {
        Vector2 dir = transform.up;
        if (PlayerPlaneController.instance != null)
        {
            Vector2 distanceVector = (PlayerPlaneController.instance.transform.position - transform.position);
            float distance = distanceVector.sqrMagnitude;
            dir = distance < _StoppingDistance ? -(Vector2)transform.right : distanceVector.normalized;
        }
        transform.up = Vector2.MoveTowards(transform.up, dir, Time.deltaTime);
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        base.FixedUpdate();
    }
}
