using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneController : PlaneController
{
    public static PlayerPlaneController instance;
    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }
    
    [SerializeField]
    private float _Acceleration;
    [SerializeField]
    private float _TurnSpeed = 1;

    private PlayerInput.PlayerActions input;

    protected override void Start()
    {
        base.Start();
        input = new PlayerInput().Player;
        input.Enable();
    }
    private void Update()
    {
        Vector2 movement = input.Movement.ReadValue<Vector2>();
        _Thrust = Mathf.Clamp(_Thrust + (movement.y * _Acceleration), 0, _MaxThrustForce);
    }
    protected override void FixedUpdate()
    {
        Vector2 movement = input.Movement.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, 0, -movement.x * _TurnSpeed));
        base.FixedUpdate();
    }
}
