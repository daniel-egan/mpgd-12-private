using UnityEngine;

public class HorizontalPlatform : MonoBehaviour
{
    public enum StartDirection { Left, Right }

    public float speed = 3f; // Speed of the platform
    public float distance = 5f; // Distance to move horizontally
    public StartDirection startDirection = StartDirection.Right; // Initial direction (from the Inspector)

    private Vector3 startPos; // Starting position
    private bool movingRight; // Direction of movement

    void Start()
    {
        startPos = transform.position; // Record the initial position

        // Set the initial direction based on the Inspector value
        if (startDirection == StartDirection.Right)
        {
            movingRight = true;
        }
        else
        {
            movingRight = false;
        }
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

    // Public method to change direction dynamically from other scripts (optional)
    public void StartMovingRight()
    {
        movingRight = true;
    }

    public void StartMovingLeft()
    {
        movingRight = false;
    }

    public void ToggleDirection()
    {
        movingRight = !movingRight;
    }
}
