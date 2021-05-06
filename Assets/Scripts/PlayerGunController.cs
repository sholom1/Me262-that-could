using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : GunController
{
    private PlayerInput.PlayerActions input;

    private bool isShooting;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        input = new PlayerInput().Player;
        input.Enable();

        input.Shoot.performed += Shoot_performed;
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isShooting = obj.ReadValueAsButton();
    }

    protected override void Update()
    {
        base.Update();
        if (isShooting)
        {
            Shoot("Enemy");
        }
    }
}
