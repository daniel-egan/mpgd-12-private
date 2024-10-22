//This script was made using this tutorial: https://youtu.be/rJqP5EesxLk?si=CMstgPC0u9ets30Y

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script handles our player moving around with the WASD keys
public class PlayerMotor : MonoBehaviour
{
    //values for speed, isGrounded, speed, gravity & jumpheight
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    //receive inputs from InputManager.cs & apply to character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        //translate WASD key input to a vector3 input that can be applied to character controller
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        //calculate the force that needs to act on the player using the vector3 input, speed & time
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        //apply a steady force of gravity on the player when they're not midair/jumping
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        //otherwise make the player fall
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        //only let the player jump if they're on the ground/a platform
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
