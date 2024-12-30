// Code used from CHATGPT
using UnityEngine;

public class CarryPlayer : MonoBehaviour
{
    public float radius = 5f; 
    public float speed = 1f; 
    public float chaseSpeed = 5f;
    public float chaseDistance = 10f; 
    public float turnSpeed = 5f; 
    private Vector3 center; 
    public float angle = 0f; 
    public bool lookingAtPlayer = false; 
    public GameObject player;


    // Initialize the centre point at which the sharks will start rotating around
    void Start()
    {
        center = transform.position; 
    }


    // The sharks will being to chase the player if within the chase distance
    // If player triggers the box collider the shark will just merely look at the player
    // Else the shark will just rotate around the prey
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= chaseDistance)
        {
            ChasePlayer(); 
        }
        else if (lookingAtPlayer)
        {
            SmoothLookAtPlayerWithOffset(); 
        }
    }


    // When player is within a certain distance from the shark the shark will rotate to look at the player
    void SmoothLookAtPlayerWithOffset()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; 
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);
        Quaternion finalRotation = lookRotation * offsetRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, turnSpeed * Time.deltaTime);
    }

    // If the player is too close to the shark the shark will chase and collide with the player
    void ChasePlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
        SmoothLookAtPlayerWithOffset();
    }


    // Shark will look at player when box collider has been triggered
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lookingAtPlayer = true;
        }
    }
}
