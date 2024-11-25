using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    public float speed = 3f; // Speed of the platform
    public float distance = 5f; // Distance to move horizontally

    private Vector3 startPos; // Starting position
    private bool movingRight = true; // Direction of movement

    void Start()
    {
        startPos = transform.position; // Record the initial position
    }

    void Update()
    {
        // Calculate the new position
        Vector3 targetPosition = startPos + (movingRight ? Vector3.right : Vector3.left) * distance;

        // Move the platform towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Reverse direction when reaching the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }
}
