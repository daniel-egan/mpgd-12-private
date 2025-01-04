//This script was made using this tutorial: https://youtu.be/rJqP5EesxLk?si=CMstgPC0u9ets30Y

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //this script acts as a manager for the input system of  the game
    private PlayerInput playerInput;
    private PlayerInput.ActionMapMainActions ActionMapMain;

    //there are separate scripts/objects for looking and moving
    private PlayerMotor motor;
    private PlayerLook look;

    public Transform resetPoint;
    public Transform checkPoint;

    // Start is called before the first frame update
    void Awake()
    {
        //recognise the player input and the action map for input
        playerInput = new PlayerInput();
        ActionMapMain = playerInput.actionMapMain;

        //set up the components for looking and moving around
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        //perform different actions based on different inputs
        ActionMapMain.Jump.performed += ctx => motor.Jump();
        ActionMapMain.Reset.performed += ctx => motor.Reset(resetPoint, checkPoint);

        ActionMapMain.WallrunUp.started += ctx => motor.isHoldingWallRunUp = true;
        ActionMapMain.WallrunUp.canceled += ctx => motor.isHoldingWallRunUp = false;

        ActionMapMain.WallrunDown.started += ctx => motor.isHoldingWallRunDown = true;
        ActionMapMain.WallrunDown.canceled += ctx => motor.isHoldingWallRunDown = false;

        ActionMapMain.Grapple.performed += ctx => motor.StartGrapple();
    }

    // Update is called once per frame
    void Update()
    {
        //tell playermotor to move using value from movement action
        motor.ProcessMove(ActionMapMain.Movement.ReadValue<Vector2>());

        //handle wallrunning for the movement system
        if (motor.isWallRunning)
        {
            look.isWallRunning = true;
        }
        else
        {
            look.isWallRunning = false;
        }
    }

    private void LateUpdate()
    {
        //tell playerlook to look using value from look action
        look.ProcessLook(ActionMapMain.Look.ReadValue<Vector2>());

    }

    //methods to turn on/off our action map
    private void OnEnable()
    {
        ActionMapMain.Enable();
    }

    private void OnDisable()
    {
        ActionMapMain.Disable();
    }
}
