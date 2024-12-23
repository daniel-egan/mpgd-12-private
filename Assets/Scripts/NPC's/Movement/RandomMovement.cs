using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;          // Movement speed of the fish
    [SerializeField] float range = 10f;         // Range for random movement in 3D space
    [SerializeField] float maxVerticalAngle = 45f; // Maximum vertical angle (degrees)
    private Vector3 targetPosition;             // Target position for the fish to move towards

    void Start()
    {
        // Set an initial random target position when the fish starts
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Rotate the fish towards the target position (both horizontally and vertically)
        RotateTowardsTarget();

        // Move the fish forward in the direction it is facing
        MoveForward();

        // If the fish has reached its target, set a new target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetRandomTargetPosition();
        }
    }

    // Rotate the fish towards the target position (both horizontally and vertically)
    void RotateTowardsTarget()
    {
        Vector3 direction = targetPosition - transform.position;  // Direction vector to the target

        // Clamp the vertical angle to be within the max allowed range (45 degrees)
        direction.y = Mathf.Clamp(direction.y, -range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle), range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle));

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);  // Calculate the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);  // Smooth rotation
        }
    }

    // Move the fish forward in the direction it's facing
    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);  // Move the fish in the direction it's facing
    }

    // Set a random target position within a specified range (random on all axes)
    void SetRandomTargetPosition()
    {
        // Generate random values for X, Y, and Z within the specified range
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);  // Can move up or down
        float z = Random.Range(-range, range);

        // Set the target position relative to the fish's current position
        targetPosition = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);

        // Clamp the vertical position to prevent the target from being too high or low
        targetPosition.y = Mathf.Clamp(targetPosition.y, transform.position.y - range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle), transform.position.y + range * Mathf.Tan(Mathf.Deg2Rad * maxVerticalAngle));
    }
}
