using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    public bool fullAuto;

    private PlayerMotor motor;
    private PlayerLook look;
    public WeaponControl weapControl;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        onFoot.Jump.performed += ctx => motor.Jump();
        if (fullAuto == true)
        {
            onFoot.Shoot.performed += ctx => weapControl.shoot();
        }
        else
        {
            onFoot.Shoot.performed += ctx => weapControl.shoot();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();   
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
