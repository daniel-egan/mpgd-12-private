using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float pushBackForce = 2f;     // Intensity of the pushback
    public float upwardForce = 1f;      // Vertical component of the pushback
    public float pushBackDuration = 1f; // Time over which the pushback occurs
    private CharacterController characterController;
    private Vector3 pushBackVelocity;   // Store the velocity of the pushback
    private float pushBackTime;         // Track remaining pushback time

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Apply pushback gradually over time
        if (pushBackTime > 0)
        {
            characterController.Move(pushBackVelocity * Time.deltaTime);
            pushBackTime -= Time.deltaTime; // Reduce remaining time
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Calculate pushback direction and add upward force
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            direction.y += upwardForce; // Add vertical force to the direction
            pushBackVelocity = direction * pushBackForce;

            // Set pushback duration
            pushBackTime = pushBackDuration;
        }
    }
}
