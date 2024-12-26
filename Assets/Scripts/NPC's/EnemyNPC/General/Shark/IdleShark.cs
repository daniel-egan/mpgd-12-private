using UnityEngine;

public class NPCRotateAroundCenter : MonoBehaviour
{
    public float radius = 5f; // Radius of the circle
    public float speed = 1f; // Speed of movement around the circle
    private Vector3 center; // Center of the circle
    private float angle = 0f; // Current angle on the circle

    void Start()
    {
        // Set the center of the circle (this could be any point in the world)
        center = transform.position;
    }

    void Update()
    {
        // Update the angle based on time and speed
        angle += speed * Time.deltaTime;

        // Keep the angle within 0 to 360 degrees range
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        // Calculate the new position using sine and cosine for X and Z axes
        float x = center.x + Mathf.Cos(angle) * radius;
        float z = center.z + Mathf.Sin(angle) * radius;

        // Update the NPC's position (staying on the circle)
        transform.position = new Vector3(x, transform.position.y, z);

        // Calculate the direction the NPC is facing
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        // Make the NPC face the direction of movement
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
