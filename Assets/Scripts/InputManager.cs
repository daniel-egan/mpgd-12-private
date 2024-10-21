using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.ActionMapMainActions ActionMapMain;

    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        ActionMapMain = playerInput.actionMapMain;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        ActionMapMain.Jump.performed += ctx => motor.Jump();
    }

    // Update is called once per frame
    void Update()
    {
        //tell playermotor to move using value from movement action
        motor.ProcessMove(ActionMapMain.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(ActionMapMain.Look.ReadValue<Vector2>());

    }

    private void OnEnable()
    {
        ActionMapMain.Enable();
    }

    private void OnDisable()
    {
        ActionMapMain.Disable();
    }
}
