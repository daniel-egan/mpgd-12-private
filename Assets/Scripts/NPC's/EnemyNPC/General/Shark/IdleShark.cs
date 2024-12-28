using UnityEngine;

public class NPCRotateAroundCenter : MonoBehaviour
{
    public float radius = 5f; // Radius of rotation around the center
    public float speed = 1f; // Speed of rotation
    public float chaseSpeed = 5f; // Speed when chasing the player
    public float chaseDistance = 10f; // Distance at which the NPC chases the player
    public float turnSpeed = 5f; // Speed of smooth turning towards the player
    private Vector3 center; // Center point of the rotation
    public float angle = 0f; // Current angle of rotation
    public bool lookingAtPlayer = false; // Flag to determine if NPC is looking at the player
    public GameObject player; // Reference to the player GameObject

    void Start()
    {
        center = transform.position; // Initialize the center point
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            ChasePlayer(); // Chase the player if within chaseDistance
        }
        else if (lookingAtPlayer)
        {
            SmoothLookAtPlayerWithOffset(); // Smoothly look at the player with an offset
        }
        else
        {
            RotateAroundCenter(); // Rotate around the center
        }
    }

    void RotateAroundCenter()
    {
        // Increment angle over time
        angle += speed * Time.deltaTime;

        // Ensure angle stays within 0-360 degrees
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        // Calculate new position based on angle
        float x = center.x + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        float z = center.z + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        transform.position = new Vector3(x, transform.position.y, z);

        // Calculate direction of movement and update rotation
        Vector3 direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle));
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void SmoothLookAtPlayerWithOffset()
    {
        // Calculate direction towards the player
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; // Ignore vertical difference

        // Calculate target rotation towards the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Apply the 90-degree offset
        Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);
        Quaternion finalRotation = lookRotation * offsetRotation;

        // Smoothly interpolate current rotation towards the target rotation with offset
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        // Move towards the player quickly
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;

        // Smoothly look at the player while chasing
        SmoothLookAtPlayerWithOffset();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = true; // Start looking at the player
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = false; // Start looking at the player
        }
    }
}
