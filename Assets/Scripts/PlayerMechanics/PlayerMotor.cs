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
    public float speed = 10f;
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
    private bool forcedDashCooldownUI = false;
    private bool isCooldownFull = false;
    public float normalFOV = 90f;
    public float dashFOV = 120f;
    public float fovTransitionSpeed = 10f;
    private Camera playerCamera;
    private Coroutine activeFOVCoroutine;

    // variables for grappling
    public float grappleSpeed = 20f; // Speed of the grapple pull
    public float maxGrappleDistance = 20f; // Maximum range of the grapple
    public LayerMask grappleLayer; // Layers the grapple can attach to
    public LineRenderer grappleLine; // Line renderer to visualize the grapple
    public bool isGrappling = false; // Whether the player is currently grappling
    private Vector3 grapplePoint; // Point the grapple is attached to
    private bool dashedBeforeGrapple = false;

    //variables for super dash
    public int instantCooldownCount = 0; // Counter for instant cooldowns
    public UnityEngine.UI.Slider superDashCounterBar; // Slider to show progress toward super dash
    public float superDashThreshold = 6; // Number of instant cooldowns needed for super dash
    public float superDashSpeed = 100f;
    private bool canSuperCount = false;
    private bool isSuperDashing = false;
    private float lastCountTime = 0f;
    public float superdashFOV = 150f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        playerCamera = Camera.main;
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFOV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            dashedBeforeGrapple = false; //Reset flag when grounded
            isGrappling = false; //Stop grappling when grounded
            grappleLine.enabled = false;
            canSuperCount = false;

            if (Time.time - lastCountTime >= 3f)
            {
                if (instantCooldownCount > 0)
                {
                    Debug.Log("RESET COOLDOWN COUNT");
                    instantCooldownCount = 0;
                    superDashCounterBar.value = 0;
                }
            }
        }

        CheckForWall();

        if (forcedDashCooldownUI)
        {
            UpdateDashCooldownUI(true);
            forcedDashCooldownUI = false;
        }
        else
        {
            UpdateDashCooldownUI();
        }

        if (isCooldownFull)
        {
            if (isDashing)
            {
                isCooldownFull = false;
            }
        }

        //if (isTouchingWall && !isGrounded && playerVelocity.y <= 0)
        if (isTouchingWall && !isGrounded)
        {
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }

        if (isDashing || isSuperDashing)
        {
            lastDashTime = Time.time;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
            if (isSuperDashing)
            {
                Debug.Log("SUPER DASH");
                controller.Move(Camera.main.transform.forward * superDashSpeed * Time.deltaTime);
                //instantCooldownCount = 0;
            }
            else
            {
                controller.Move(Camera.main.transform.forward * dashSpeed * Time.deltaTime);
            }
        }

        if (isGrappling)
        {
            PerformGrapple();
        }

        //Handle upwards and downwards wallrunning
        if (isHoldingWallRunUp && isWallRunning)
        {
            WallRunUp();
        }
        else if (isHoldingWallRunDown && isWallRunning)
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
        if (!isWallRunning && !isDashing)
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
            canSuperCount = true;

            if (instantCooldownCount >= superDashThreshold)
            {
                instantCooldownCount = 0;
                isSuperDashing = true;
                if (dashCoroutine != null)
                {
                    StopCoroutine(dashCoroutine);
                }
                dashCoroutine = StartCoroutine(PerformSuperDash());
            }
            else
            {
                if (dashCoroutine != null)
                {
                    StopCoroutine(dashCoroutine);
                }
                dashCoroutine = StartCoroutine(PerformDash());
            }
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
            if (Vector3.Dot(transform.forward, hitRight.normal) < -0.1f)
            {
                isTouchingWall = true;
                wallNormal = hitRight.normal; // Get the normal of the wall on the right
            }

        }
        else if (Physics.Raycast(transform.position, -transform.right, out hitLeft, wallCheckDistance, wallLayer))
        {
            if (Vector3.Dot(transform.forward, hitLeft.normal) < -0.1f)
            {
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
        canSuperCount = false;

        // Cancel gravity while wall running
        if (!isHoldingWallRunUp && !isHoldingWallRunDown)
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
        dashedBeforeGrapple = true; // Set the flag when dashing starts

        //lastDashTime = Time.time;

        StartFOVTransition(dashFOV);
        yield return new WaitForSeconds(dashDuration);
        StartFOVTransition(normalFOV);

        isDashing = false;

        // Wait for the cooldown if it hasn't been reset by grappling
        if (!isGrappling)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
            forcedDashCooldownUI = true;
        }
    }

    private IEnumerator PerformSuperDash()
    {
        canDash = false;
        isSuperDashing = true;

        dashCooldownBar.value = 0f;
        superDashCounterBar.value = 0f;
        instantCooldownCount = 0;

        StartFOVTransition(superdashFOV);
        yield return new WaitForSeconds(dashDuration);
        StartFOVTransition(normalFOV);

        isSuperDashing = false;

        if (!isGrappling)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
            forcedDashCooldownUI = true;
        }
    }

    private IEnumerator ChangeFOV(float targetFOV)
    {
        if (playerCamera == null) yield break;

        while (Mathf.Abs(playerCamera.fieldOfView - targetFOV) > 0.1f)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV; // Ensure the FOV reaches the exact value
    }

    private void StartFOVTransition(float targetFOV)
    {
        // Stop the current FOV coroutine if it's active
        if (activeFOVCoroutine != null)
        {
            StopCoroutine(activeFOVCoroutine);
        }

        // Start a new FOV coroutine and track it
        activeFOVCoroutine = StartCoroutine(ChangeFOV(targetFOV));
    }


    void UpdateDashCooldownUI(bool forceUpdate = false)
    {
        float counterProgress;

        if (forceUpdate)
        {
            // Instantly set slider to 1 (fully cooled down) and increment superDashCounter
            dashCooldownBar.value = 1f;
            isCooldownFull = true;

            if (canSuperCount)
            {
                instantCooldownCount++;
                lastCountTime = Time.time;
            }

            //show super dash counter progress
            counterProgress = Mathf.Clamp01((float)instantCooldownCount / superDashThreshold);
            superDashCounterBar.value = counterProgress;

            Canvas.ForceUpdateCanvases();
            return;
        }

        if (isCooldownFull)
        {
            return;
        }

        //show dash progress
        float cooldownProgress = Mathf.Clamp01((Time.time - lastDashTime) / dashCooldown);
        dashCooldownBar.value = cooldownProgress;

        //show super dash counter progress
        counterProgress = Mathf.Clamp01((float)instantCooldownCount / superDashThreshold);
        superDashCounterBar.value = counterProgress;
    }

    public void StartGrapple()
    {
        if (isGrappling)
        {
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;

            // Prevent grappling upwards
            if (grapplePoint.y > transform.position.y) return;

            isGrappling = true;

            // Visualize the grapple
            grappleLine.enabled = true;
            grappleLine.SetPosition(0, transform.position);
            grappleLine.SetPosition(1, grapplePoint);

            // Reset the dash cooldown if dashed before grappling
            if (dashedBeforeGrapple)
            {
                canDash = true; // Reset dash availability
                dashedBeforeGrapple = false; // Reset the flag
                forcedDashCooldownUI = true;
            }
        }
    }



    void PerformGrapple()
    {
        Vector3 direction = grapplePoint - transform.position;
        float distance = direction.magnitude;

        if (distance < 1f || isGrounded)
        {
            StopGrapple();
            return;
        }

        controller.Move(direction.normalized * grappleSpeed * Time.deltaTime);

        // Update the grapple visualization
        grappleLine.SetPosition(0, transform.position);
        grappleLine.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        isGrappling = false;
        grappleLine.enabled = false;

        if (dashedBeforeGrapple) // Ensure a dash occurred before the grapple
        {
            canSuperCount = true; // Allow the counter to increment
            dashedBeforeGrapple = false; // Reset the flag to prevent multiple increments

            // Increment the super dash counter and update the UI
            instantCooldownCount++;
            lastCountTime = Time.time;
            superDashCounterBar.value = Mathf.Clamp01((float)instantCooldownCount / superDashThreshold);

            Debug.Log("Super Dash Counter Incremented!");
        }

        // Snap player to the ground after grappling
        if (!isGrounded)
        {
            Ray groundRay = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(groundRay, out RaycastHit hit, 2f, grappleLayer))
            {
                controller.Move(Vector3.down * (hit.distance - 0.1f)); // Slight adjustment to ground level
            }
        }

        // Reset vertical velocity
        playerVelocity.y = -2f;

        // Update grounded state
        isGrounded = controller.isGrounded;
    }


}