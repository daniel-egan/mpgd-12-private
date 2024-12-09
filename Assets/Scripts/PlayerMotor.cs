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

    //variables for wallrunning
    public bool isWallRunning = false;
    public float wallRunGravity = -2f;
    public float wallRunSpeed = 5f;
    public LayerMask wallLayer;
    public float wallCheckDistance = 1f;
    public float wallRunJumpForce = 8f;
    public RaycastHit hitRight;
    public RaycastHit hitLeft;

    private bool isTouchingWall = false;
    public Vector3 wallNormal;
    public float wallRunJumpLateralForce = 8f;
    public bool isHoldingWallRunUp = false;
    public bool isHoldingWallRunDown = false;

    //variables for dashing
    public float dashSpeed = 40f; // Speed of the dash
    public float dashDuration = 0.2f; // How long the dash lasts
    public float dashCooldown = 2f; // Cooldown time for the dash
    private bool canDash = true; // Whether the player can dash
    private bool isDashing = false;
    private Coroutine dashCoroutine;
    public UnityEngine.UI.Slider dashCooldownBar;
    private float lastDashTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerVelocity.y);
        //Debug.Log(isTouchingWall);
        isGrounded = controller.isGrounded;

        CheckForWall();
        UpdateDashCooldownUI();

        //if (isTouchingWall && !isGrounded && playerVelocity.y <= 0)
        if (isTouchingWall && !isGrounded)
        {
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }

        if (isDashing)
        {
            lastDashTime = Time.time;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
            controller.Move(Camera.main.transform.forward * dashSpeed * Time.deltaTime);
        }

        //Handle upwards and downwards wallrunning
        if (isHoldingWallRunUp)
        {
            WallRunUp();
        }
        else if (isHoldingWallRunDown)
        {
            WallRunDown();
        }
        else if (isWallRunning)
        {
            playerVelocity.y = 0;
        }

        // Check if the player is jumping and collides with something above
        if (!isGrounded && playerVelocity.y > 0)
        {
            // Check for collisions above using the CharacterController's collision flags
            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                // Immediately stop upward velocity and start falling
                playerVelocity.y = 0;
            }
        }
    }

    //receive inputs from InputManager.cs & apply to character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        //translate WASD key input to a vector3 input that can be applied to character controller
        moveDirection.x = input.x;
        moveDirection.z = input.y;


        //calculate the force that needs to act on the player using the vector3 input, speed & time, given that they're not wallrunning
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        //apply a steady force of gravity on the player when they're not midair/jumping
        if (!isWallRunning)
        {
            playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
        }

        //otherwise make the player fall, if they're not wallrunning
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        else if (isWallRunning)
        {
            // Jump off the wall
            float upwardForce = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            Vector3 forwardForce = transform.forward * wallRunJumpForce;
            Vector3 lateralForce = wallNormal * wallRunJumpLateralForce;

            /*Debug.Log($"{forwardForce}");
            Debug.Log($"{lateralForce}");
            Debug.Log($"{wallNormal}");*/

            Vector3 wallJumpForce = forwardForce + lateralForce + (Vector3.up * upwardForce);
            playerVelocity += wallJumpForce;

            //playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            StopWallRun();
        }
        else if (canDash)
        {
            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
            }
            dashCoroutine = StartCoroutine(PerformDash());
        }
    }


    public void Reset(Transform resetPosition, Transform checkPoint)
    {
        controller.enabled = false;
        if (gameObject.GetComponent<Player>().hasKey == true)
        {
            controller.transform.position = checkPoint.position;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
        }
        else
        {
            controller.transform.position = resetPosition.position;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
        }
        controller.enabled = true;
    }

    void CheckForWall()
    {
        // Check if either wall is detected
        if (Physics.Raycast(transform.position, transform.right, out hitRight, wallCheckDistance, wallLayer))
        {
            if(Vector3.Dot(transform.forward, hitRight.normal) < -0.1f)
            {
                Debug.Log("Touching Wall");
                isTouchingWall = true;
                wallNormal = hitRight.normal; // Get the normal of the wall on the right
            }

        }
        else if (Physics.Raycast(transform.position, -transform.right, out hitLeft, wallCheckDistance, wallLayer))
        {
            if(Vector3.Dot(transform.forward, hitLeft.normal) < -0.1f)
            {
                Debug.Log("Touching Wall");
                isTouchingWall = true;
                wallNormal = hitLeft.normal; // Get the normal of the wall on the left
            }

        }
        else
        {
            isTouchingWall = false;
        }
    }


    void StartWallRun()
    {
        if (playerVelocity.y > 0 && !isHoldingWallRunUp && !isHoldingWallRunDown) return; // Ignore wall running if jumping

        isWallRunning = true;

        // Cancel gravity while wall running
        if(!isHoldingWallRunUp && !isHoldingWallRunDown)
        {
            playerVelocity.y = 0;
        }

        // Determine the movement direction along the wall
        Vector3 wallRunDirection = Vector3.Cross(wallNormal, Vector3.up); // Move perpendicular to the wall

        // If the wall is to the right, we want the player to move to the left along the wall
        if (wallNormal == hitRight.normal)
        {
            wallRunDirection = -wallRunDirection; // Invert direction if the wall is on the right
        }

        // Apply the movement
        controller.Move(wallRunDirection * wallRunSpeed * Time.deltaTime);
    }

    public void WallRunUp()
    {
        Debug.Log("Wallrun Up");
        if (isWallRunning && isHoldingWallRunUp)
        {
            playerVelocity.y = Mathf.Lerp(playerVelocity.y, wallRunSpeed, Time.deltaTime * 10f); // Smoothly increase upward speed
        }
        else
        {
            playerVelocity.y = 0;
        }
    }

    public void WallRunDown()
    {
        Debug.Log("Wallrun Down");
        if (isWallRunning && isHoldingWallRunDown)
        {
            playerVelocity.y = Mathf.Lerp(playerVelocity.y, -wallRunSpeed, Time.deltaTime * 10f); // Smoothly decrease upward speed
        }
        else
        {
            playerVelocity.y = 0;
        }
    }

    void StopWallRun()
    {
        isWallRunning = false;

        if (isGrounded)
        {
            playerVelocity.x = 0;
            playerVelocity.z = 0;
        }

        // Apply gravity only if the player is not jumping
        if (playerVelocity.y <= 0)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Start cooldown
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void UpdateDashCooldownUI()
    {
        // Calculate the cooldown progress (1 = ready, 0 = just used)
        float cooldownProgress = Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);

        // Update the slider's value
        dashCooldownBar.value = cooldownProgress;
    }
}
