using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;               // Normal movement speed of the fish
    [SerializeField] float escapeSpeedMultiplier = 2f; // Speed multiplier when escaping
    [SerializeField] float range = 10f;              // Range for random movement in 3D space
    [SerializeField] float maxVerticalAngle = 45f;   // Maximum vertical angle (degrees)
    [SerializeField] float minimumDistance = 5f;    // Minimum distance from any obstacle
    [SerializeField] float raycastDistance = 10f;    // Distance for raycasting
    private Vector3 targetPosition;                 // Target position for the fish to move towards
    private bool isAvoiding = false;                 // Is the fish currently avoiding?
    private float originalSpeed;                     // Store the original speed
    private float escapeStartDistance;               // Distance from when escape started

    void Start()
    {
        originalSpeed = speed;                      // Save the original speed
        SetRandomTargetPosition();                   // Set initial random target
    }

    void Update()
    {
        RaycastCheck(); // Continuously check raycast for obstacle proximity

        if (!isAvoiding)
        {
            // Rotate towards target position and move normally
            RotateTowardsTarget();
            MoveForward();

            // Set a new target if the current target is reached
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                SetRandomTargetPosition();
            }
        }
        else
        {
            // Continue escaping until we are 2x the minimum distance away from any object
            if (Vector3.Distance(transform.position, targetPosition) >= escapeStartDistance * 2)
            {
                // Once the fish is far enough away, stop escaping
                isAvoiding = false;
                speed = originalSpeed; // Reset speed back to normal
            }
            else
            {
                MoveForward(); // Keep moving while avoiding
            }
        }
    }

    // Rotate the fish towards the target position
    void RotateTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;

        // Clamp vertical angle
        direction.y = Mathf.Clamp(direction.y, -range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle), range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle));

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    // Move the fish forward
    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Set a random target position within range
    void SetRandomTargetPosition()
    {
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        float z = Random.Range(-range, range);

        targetPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        targetPosition.y = Mathf.Clamp(targetPosition.y, transform.position.y - range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle), transform.position.y + range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle));
    }

    // Use raycasting to check distance from all obstacles
    void RaycastCheck()
    {
        // Cast a ray in all directions (forward, backward, etc.) to check for proximity
        RaycastHit hit;

        // Check in the forward direction
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            HandleEscape(hit.transform);
        }




        // Check in the backward direction
        if (Physics.Raycast(transform.position, -transform.forward, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Player"))  // Check if the raycast hit the Player
            {
                HandleEscape(hit.transform);
            }
        }

        // Check to the left
        if (Physics.Raycast(transform.position, -transform.right, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Player"))  // Check if the raycast hit the Player
            {
                HandleEscape(hit.transform);
            }
        }

        // Check to the right
        if (Physics.Raycast(transform.position, transform.right, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Player"))  // Check if the raycast hit the Player
            {
                HandleEscape(hit.transform);
            }
        }

        // Check upwards
        if (Physics.Raycast(transform.position, transform.up, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Player"))  // Check if the raycast hit the Player
            {
                HandleEscape(hit.transform);
            }
        }

        // Check downwards
        if (Physics.Raycast(transform.position, -transform.up, out hit, raycastDistance))
        {
            if (hit.transform.CompareTag("Player"))  // Check if the raycast hit the Player
            {
                HandleEscape(hit.transform);
            }
        }





    }

    // Handle escaping if an obstacle is detected
    void HandleEscape(Transform detectedTransform)
    {
        // If the fish detects an obstacle and it is within the minimum distance, escape
        if (Vector3.Distance(transform.position, detectedTransform.position) < minimumDistance)
        {
            // Avoid the obstacle by moving in the opposite direction
            Vector3 escapeDirection = (transform.position - detectedTransform.position).normalized;

            // Set a new target position away from the obstacle
            targetPosition = transform.position + escapeDirection * range;

            // Store the current distance when the escape begins
            escapeStartDistance = Vector3.Distance(transform.position, detectedTransform.position);

            // Rotate to face away from the obstacle
            Quaternion escapeRotation = Quaternion.LookRotation(escapeDirection);
            transform.rotation = escapeRotation;

            // Temporarily increase speed for escape
            speed = originalSpeed * escapeSpeedMultiplier;

            // Mark as avoiding
            isAvoiding = true;

            Debug.Log($"Escaping from: {detectedTransform.name} (New target: {targetPosition})");
        }
    }
}