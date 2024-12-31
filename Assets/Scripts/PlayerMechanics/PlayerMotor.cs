//This script was made using this tutorial: https://youtu.be/rJqP5EesxLk?si=CMstgPC0u9ets30Y

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

//this script handles all player movement mechanics
public class PlayerMotor : MonoBehaviour
{
    //fundamental values (for speed, isGrounded, speed, gravity & jumpheight)
    private CharacterController controller;
    private Vector3 playerVelocity;
    public bool isGrounded;
    public float speed = 10f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    //variables for wallrunning
    public bool isWallRunning = false;
    public bool WallRunningSoundOn = false;
    public float wallRunGravity = -2f;
    public float wallRunSpeed = 5f;
    public LayerMask wallLayer;
    public float wallCheckDistance = 1f;
    public float wallRunJumpForce = 8f;
    public RaycastHit hitRight;
    public RaycastHit hitLeft;
    private Rigidbody rb;
    private bool isTouchingWall = false;
    public Vector3 wallNormal;
    public float wallRunJumpLateralForce = 8f;
    public bool isHoldingWallRunUp = false;
    public bool isHoldingWallRunDown = false;

    //variables for dashing
    public float dashSpeed = 40f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    private bool canDash = true;
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

    //variables for super dash
    public int instantCooldownCount = 0;
    public UnityEngine.UI.Slider superDashCounterBar;
    public float superDashThreshold = 6;
    public float superDashSpeed = 100f;
    private bool canSuperCount = false;
    private bool isSuperDashing = false;
    private float lastCountTime = 0f;
    public float superdashFOV = 150f;

    // variables for grappling
    public float grappleSpeed = 20f;
    public float maxGrappleDistance = 20f;
    public LayerMask grappleLayer;
    public LineRenderer grappleLine;
    public bool isGrappling = false;
    private Vector3 grapplePoint;
    private bool dashedBeforeGrapple = false;
    public float grappleFOV = 100f;
    public bool grappleDashBan = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        playerCamera = Camera.main;

        //set up FOV manipulation for later
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = normalFOV;
        }

        //unlock rotation from Rigidbody (for wallrun tilt) but lock Y axis to prevent rolling camera
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        //Handle SD Loss and Grapple disable when grounded
        if (isGrounded)
        {
            dashedBeforeGrapple = false;
            isGrappling = false;
            grappleLine.enabled = false;
            canSuperCount = false;

            if (Time.time - lastCountTime >= 3f)
            {
                if (instantCooldownCount > 0)
                {
                    SoundManager.Instance.PlaySDLoss();
                    instantCooldownCount = 0;
                    superDashCounterBar.value = 0;
                }
            }
        }

        //Always check for wall for wallrunning
        CheckForWall();

        //Handle UI refresh for SD buildup/regular refresh
        if (forcedDashCooldownUI)
        {
            UpdateDashCooldownUI(true);
            forcedDashCooldownUI = false;
        }
        else
        {
            UpdateDashCooldownUI();
        }

        //Enable cooldown period once dashed
        if (isCooldownFull)
        {
            if (isDashing)
            {
                isCooldownFull = false;
            }
        }

        //Wallrun if touching wall and midair
        if (isTouchingWall && !isGrounded)
        {
            StartWallRun();
        }
        else
        {
            StopWallRun();
        }

        //Handle dashing & super dashing
        if (isDashing || isSuperDashing)
        {
            lastDashTime = Time.time;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
            if (isSuperDashing)
            {
                controller.Move(Camera.main.transform.forward * superDashSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(Camera.main.transform.forward * dashSpeed * Time.deltaTime);
            }
        }

        //Handle grappling
        if (isGrappling)
        {
            PerformGrapple();
        }

        if (isGrappling && canDash)
        {
            canDash = false;
            grappleDashBan = true;
        }
        
        if (!isGrappling && !canDash && grappleDashBan)
        {
            canDash = true;
            grappleDashBan = false;
        }

        //Handle wallrunning (upwards and downwards too)
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

        //Handle wallrunning sound
        if (isWallRunning)
        {
            if (!WallRunningSoundOn)
            {
                SoundManager.Instance.wallRunningSource.loop = true;
                SoundManager.Instance.wallRunningSource.Play();
                WallRunningSoundOn = true;
            }
        }
        else
        {
            if (WallRunningSoundOn)
            {
                SoundManager.Instance.wallRunningSource.Stop();
                WallRunningSoundOn = false;
            }
        }

        // Prevent "hovering" if player collides something whilst jumping
        if (!isGrounded && playerVelocity.y > 0)
        {
            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
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


        //calculate the force that needs to act on the player using the vector3 input, speed & time
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

    //handle all "jumping" (Spacebar inputs)
    public void Jump()
    {
        //regular jump
        if (isGrounded)
        {
            SoundManager.Instance.PlayJump();
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        //wallrunning jump
        else if (isWallRunning)
        {
            // Jump off the wall
            float upwardForce = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            Vector3 forwardForce = transform.forward * wallRunJumpForce;
            Vector3 lateralForce = wallNormal * wallRunJumpLateralForce;

            SoundManager.Instance.PlayJump();
            Vector3 wallJumpForce = forwardForce + lateralForce + (Vector3.up * upwardForce);
            playerVelocity += wallJumpForce;

            StopWallRun();
        }
        //dash and superdash
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
                SoundManager.Instance.PlayDash();
                dashCoroutine = StartCoroutine(PerformDash());
            }
        }
    }

    //handle resets
    public void Reset(Transform resetPosition, Transform checkPoint)
    {
        SoundManager.Instance.PlayReset();
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

    //check for wall on either side of player
    void CheckForWall()
    {
        // Use raycasting, then if wall detected, retrieve the normal of the wall (to wallrun in right direction)
        if (Physics.Raycast(transform.position, transform.right, out hitRight, wallCheckDistance, wallLayer))
        {
            if (Vector3.Dot(transform.forward, hitRight.normal) < -0.1f)
            {
                isTouchingWall = true;
                wallNormal = hitRight.normal;
            }

        }
        else if (Physics.Raycast(transform.position, -transform.right, out hitLeft, wallCheckDistance, wallLayer))
        {
            if (Vector3.Dot(transform.forward, hitLeft.normal) < -0.1f)
            {
                isTouchingWall = true;
                wallNormal = hitLeft.normal;
            }

        }
        else
        {
            isTouchingWall = false;
        }
    }

    //wallrun if wall is on either side of player
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

    //handle wallrunning upwards
    public void WallRunUp()
    {
        if (isWallRunning && isHoldingWallRunUp)
        {
            playerVelocity.y = Mathf.Lerp(playerVelocity.y, wallRunSpeed, Time.deltaTime * 10f); // Smoothly increase upward speed
        }
        else
        {
            playerVelocity.y = 0;
        }
    }

    //handle wallrunning downwards
    public void WallRunDown()
    {
        if (isWallRunning && isHoldingWallRunDown)
        {
            playerVelocity.y = Mathf.Lerp(playerVelocity.y, -wallRunSpeed, Time.deltaTime * 10f); // Smoothly decrease upward speed
        }
        else
        {
            playerVelocity.y = 0;
        }
    }

    //stop wallrunning
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

    //handle dashing
    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        dashedBeforeGrapple = true; // Enable for potential SD buildup

        //manipulate FOV to give "speed" effect when dashing
        StartFOVTransition(dashFOV);
        yield return new WaitForSeconds(dashDuration);
        StartFOVTransition(normalFOV);

        isDashing = false;

        // Wait for regular cooldown if it hasn't been reset by grappling
        if (!isGrappling)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
            forcedDashCooldownUI = true;
        }
    }

    //handle superdash
    private IEnumerator PerformSuperDash()
    {
        //disable regular dashing as well to ensure it's reset from 0 (to prevent player double dashing)
        canDash = false;
        isSuperDashing = true;
        SoundManager.Instance.PlaySuperDash();

        //reset SD buildup values to 0
        dashCooldownBar.value = 0f;
        superDashCounterBar.value = 0f;
        instantCooldownCount = 0;


        //manipulate FOV to give "speed" effect when superdashing
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

    //Handle FOV changes
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

    //Helper function to help with FOV changes
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

    //handle dash and superdash UI updates
    void UpdateDashCooldownUI(bool forceUpdate = false)
    {
        float counterProgress;

        //forceUpdate indicates that SD buildup has occurred
        if (forceUpdate)
        {
            // Instantly set slider to 1 (fully cooled down) and increment superDashCounter
            dashCooldownBar.value = 1f;
            isCooldownFull = true;

            //Incrementing SD buildup
            if (canSuperCount)
            {
                SoundManager.Instance.PlaySDBuildup();
                instantCooldownCount++;
                lastCountTime = Time.time;
                if (instantCooldownCount >= superDashThreshold)
                {
                    SoundManager.Instance.PlaySDUnlock();
                }
            }

            //show super dash counter progress once incremented
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

    //start grappling
    public void StartGrapple()
    {
        if (isGrappling)
        {
            return;
        }

        //visualise "grapple" wire with raycast ray
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrappleDistance, grappleLayer))
        {
            grapplePoint = hit.point;

            // Prevent grappling upwards
            if (grapplePoint.y > transform.position.y) return;

            isGrappling = true;
            SoundManager.Instance.PlayGrapple();

            // Visualize the grapple
            grappleLine.enabled = true;
            grappleLine.SetPosition(0, transform.position);
            grappleLine.SetPosition(1, grapplePoint);

            StartFOVTransition(grappleFOV);

            // Reset the dash cooldown if dashed before grappling
            if (dashedBeforeGrapple)
            {
                canDash = true; // Reset dash availability
                dashedBeforeGrapple = false; // Reset the flag
                forcedDashCooldownUI = true;
            }
        }
    }

    //in the middle of grappling
    void PerformGrapple()
    {
        //calculate direciton needed to go in for grapple
        Vector3 direction = grapplePoint - transform.position;
        float distance = direction.magnitude;

        //stop grappling once at the target
        if (distance < 1f || isGrounded)
        {
            StopGrapple();
            return;
        }

        //move player towards grapple target
        controller.Move(direction.normalized * grappleSpeed * Time.deltaTime);

        // Update the grapple visualization
        grappleLine.SetPosition(0, transform.position);
        grappleLine.SetPosition(1, grapplePoint);
    }

    //at end of grapple
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

        StartFOVTransition(normalFOV);

        // Update grounded state
        isGrounded = controller.isGrounded;
    }


}